using AutoMapper;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.DataAccess.Preset;

using Preset = Core.Overclocking.Preset;

/// <summary>
/// Реализация <see cref="IPresetRepository"/>.
/// </summary>
public class PresetRepository : IPresetRepository
{
    private readonly Context _context;

    private readonly IMapper _mapper;

    public PresetRepository(Context context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Preset> GetAllAvailable(string? gpuName, Specification specification)
    {
        var presets = from preset in _context.Presets.AsNoTrackingWithIdentityResolution()
                                                     .Where(x => x.UserId == specification.UserId && x.IsVisible)
                                                     .Filter(specification)
                      join overclocking in _context.Overclocking.AsNoTracking()
                        on preset.OverclockingId equals overclocking.Id
                      select new Preset()
                      {
                          Id = preset.Id,
                          UserId = preset.UserId,
                          Name = preset.Name,
                          DeviceName = preset.DeviceName,
                          OverclockingId = overclocking.Id,
                          Overclocking = _mapper.Map<IOverclocking>(overclocking)
                      };

        return string.IsNullOrWhiteSpace(gpuName) 
            ? presets.AsAsyncEnumerable()
            : presets.Where(x => x.DeviceName == gpuName).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<IGrouping<string, Preset>> GetGroupedList(Expression<Func<Preset, string>> expression,
                                                                      Specification specification)
    {
        var presets = from preset in _context.Presets.AsNoTrackingWithIdentityResolution()
                                                     .Where(x => x.UserId == specification.UserId && x.IsVisible)
                                                     .Filter(specification)
                      join overclocking in _context.Overclocking.AsNoTracking()
                        on preset.OverclockingId equals overclocking.Id
                      select new Preset()
                      {
                          Id = preset.Id,
                          UserId = preset.UserId,
                          Name = preset.Name,
                          DeviceName = preset.DeviceName,
                          OverclockingId = overclocking.Id,
                          Overclocking = _mapper.Map<IOverclocking>(overclocking)
                      };

        return presets.GroupBy(expression).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Preset?> GetAvailableById(Guid id,
                                          Guid userId,
                                          CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
            .Where(x => x.Id == id && x.UserId == userId && x.IsVisible)
            .Join(_context.Overclocking.AsNoTracking(),
            preset => preset.OverclockingId,
            overclocking => overclocking.Id,
            (preset, overclocking) => new Preset
            {
                Id = preset.Id,
                Name = preset.Name,
                DeviceName = preset.DeviceName,
                OverclockingId = preset.OverclockingId,
                Overclocking = _mapper.Map<IOverclocking>(overclocking)
            }).FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId,
                             string name,
                             CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
            .Where(x => x.UserId == userId && x.IsVisible)
            .AnyAsync(x => x.Name.Equals(name), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Save(Preset preset)
    {
        var overclocking = _mapper.
            Map<OverclockingDto>(preset.Overclocking);
        await _context.Overclocking.AddAsync(overclocking);

        await _context.Presets.AddAsync(preset);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Update(Preset preset)
    {
        var oldOverclocking = await _context.Overclocking
            .FirstOrDefaultAsync(x => x.Id == preset.OverclockingId);
        if (oldOverclocking != null)
        {
            _context.Overclocking.Remove(oldOverclocking);
        }

        var overclocking = _mapper.Map<OverclockingDto>(preset.Overclocking);
        await _context.Overclocking.AddAsync(overclocking);

        preset.OverclockingId = overclocking.Id;
        _context.Presets.Update(preset);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Remove(Guid id, Guid userId)
    {
        var preset = await _context.Presets.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.UserId == userId && x.IsVisible);

        if (preset is null) return;

        var removableOverclocking = await _context.Overclocking
            .FirstOrDefaultAsync(x => x.Id == preset.OverclockingId);

        if (removableOverclocking is null) return;

        var devices = await _context.MiningDevices
            .Where(x => x.PresetId == preset.Id)
            .ToListAsync();

        if (devices.Any())
        {
            var invisiblePresets = await _context.Presets
                .Where(x => !x.IsVisible && devices
                .Select(d => d.Id.ToString()).Contains(x.Name))
                    .ToListAsync();

            foreach (var device in devices)
            {
                var invisiblePreset = invisiblePresets
                    .FirstOrDefault(
                        x => x.Name == device.Id.ToString() &&
                        !x.IsVisible);

                if (invisiblePreset is null) continue;

                var overclocking = await _context.Overclocking
                    .FirstOrDefaultAsync(x => x.Id == invisiblePreset.OverclockingId);

                if (overclocking is not null)
                {
                    UpdateOverclockingParams(overclocking, removableOverclocking);
                }

                device.PresetId = invisiblePreset.Id;    
            }

            
        }
        _context.Overclocking.Remove(removableOverclocking);
        _context.Presets.Remove(preset);

        await _context.SaveChangesAsync();
    }

    private void UpdateOverclockingParams(OverclockingDto target, OverclockingDto source)
    {
        target.CoreClockLock = source.CoreClockLock;
        target.CoreClockOffset = source.CoreClockOffset;
        target.MemoryClockLock = source.MemoryClockLock;
        target.MemoryClockOffset = source.MemoryClockOffset;
        target.CoreVoltage = source.CoreVoltage;
        target.CoreVoltageOffset = source.CoreVoltageOffset;
        target.MemoryVoltage = source.MemoryVoltage;
        target.MemoryVoltageOffset = source.MemoryVoltageOffset;
        target.PowerLimit = source.PowerLimit;
        target.FanSpeed = source.FanSpeed;
    }
}