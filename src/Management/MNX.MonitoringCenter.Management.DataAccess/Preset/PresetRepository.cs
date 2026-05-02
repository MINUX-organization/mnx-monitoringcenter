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
                                                     .Where(x => x.OwnerId == specification.UserId && x.IsVisible)
                                                     .Filter(specification)
                                                     .Sort()
                      join overclocking in _context.Overclocking.AsNoTracking().Include(x => x.FanOverclocking)
                        on preset.OverclockingId equals overclocking.Id
                      select new Preset()
                      {
                          Id = preset.Id,
                          OwnerId = preset.OwnerId,
                          Name = preset.Name,
                          DeviceName = preset.DeviceName,
                          IsVisible = preset.IsVisible,
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
                                                     .Where(x => x.OwnerId == specification.UserId && x.IsVisible)
                                                     .Filter(specification)
                      join overclocking in _context.Overclocking.AsNoTracking().Include(x => x.FanOverclocking)
                        on preset.OverclockingId equals overclocking.Id
                      select new Preset()
                      {
                          Id = preset.Id,
                          OwnerId = preset.OwnerId,
                          Name = preset.Name,
                          DeviceName = preset.DeviceName,
                          IsVisible = preset.IsVisible,
                          OverclockingId = overclocking.Id,
                          Overclocking = _mapper.Map<IOverclocking>(overclocking)
                      };

        return presets.GroupBy(expression).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Preset?> GetById(Guid id,
                                 Guid userId,
                                 CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
            .Where(x => x.Id == id && x.OwnerId == userId)
            .Join(_context.Overclocking.AsNoTracking().Include(x => x.FanOverclocking),
            preset => preset.OverclockingId,
            overclocking => overclocking.Id,
            (preset, overclocking) => new Preset
            {
                Id = preset.Id,
                Name = preset.Name,
                DeviceName = preset.DeviceName,
                OverclockingId = preset.OverclockingId,
                Overclocking = _mapper.Map<IOverclocking>(overclocking),
                OwnerId = preset.OwnerId,
                IsVisible = preset.IsVisible,
            }).FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId,
                             string name,
                             CancellationToken cancellationToken)
    {
        return _context.Presets.AsNoTracking()
            .Where(x => x.OwnerId == userId && x.IsVisible)
            .AnyAsync(x => x.Name.Equals(name), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Save(Preset preset)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync();

        var overclocking = _mapper.
            Map<OverclockingDto>(preset.Overclocking);
        await _context.Overclocking.AddAsync(overclocking);
        await _context.SaveChangesAsync();

        await _context.Presets.AddAsync(preset);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
    }

    /// <inheritdoc/>
    public Task Update(Preset preset)
    {
        var overclocking = _mapper.Map<OverclockingDto>(preset.Overclocking);
        _context.Overclocking.Update(overclocking);
        _context.Presets.Update(preset);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task Remove(Guid id, Guid userId)
    {
        var removablePreset = await _context.Presets.AsNoTracking()
            .Where(x => x.Id == id && x.OwnerId == userId && x.IsVisible)
            .FirstOrDefaultAsync();

        if (removablePreset == null) return;

        var overclockingId = removablePreset.OverclockingId;

        await _context.Presets.AsNoTracking()
            .Where(x => x.Id == id && x.OwnerId == userId && x.IsVisible)
            .ExecuteDeleteAsync();

        await _context.Overclocking.AsNoTracking()
            .Where(x => x.Id == overclockingId)
            .ExecuteDeleteAsync();
    }
}