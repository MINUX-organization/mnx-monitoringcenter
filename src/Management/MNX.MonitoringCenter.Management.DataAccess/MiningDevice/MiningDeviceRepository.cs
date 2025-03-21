using AutoMapper;
using System.Data;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/>.
/// </summary>
public class MiningDeviceRepository : IMiningDeviceRepository
{
    private readonly IMapper _mapper;

    private readonly Context _context;

    public MiningDeviceRepository(Context context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MiningDeviceInfo> GetAvailable(Specification specification)
    {
        return GetDevicesQuery(specification).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<List<MiningDeviceInfo>> GetAvailableByPresetId(Guid presetId,
                                                                     Guid userId)
    {
        return await _context.MiningDevices.AsNoTracking().Where(
            x => x.OwnerId == userId && x.PresetId == presetId).ToListAsync();
    }

    /// <inheritdoc/>
    public Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id,
                                                       Guid userId,
                                                       CancellationToken cancellationToken)
    {
        return _context.MiningDevices.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id && x.OwnerId == userId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name, Guid userId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices
                       .AsNoTracking()
                       .Where(device => device.OwnerId == userId)
                       .AnyAsync(device => (device.Manufacturer + ' ' + device.Model) == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetPreset(Guid presetId,
                                CancellationToken cancellationToken,
                                params Guid[] deviceIds)
    {
        var devicesToUpdate = await _context.MiningDevices
            .Where(x => deviceIds.Contains(x.Id))
            .ToListAsync();

        foreach (var device in devicesToUpdate)
        {
            device.PresetId = presetId;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetOverclocking(MiningDeviceInfo device, IOverclocking overclocking)
    {
        var invisiblePreset = await _context.Presets
            .FirstOrDefaultAsync(x => x.Name == device.Id.ToString() && !x.IsVisible);

        if (invisiblePreset == null) return;

        var oldOverclocking = await _context.Overclocking
            .FirstOrDefaultAsync(x => x.Id == invisiblePreset.OverclockingId);

        if (oldOverclocking != null)
        {
            _context.Overclocking.Remove(oldOverclocking);
        }

        var dto = _mapper.Map<OverclockingDto>(overclocking);

        invisiblePreset.OverclockingId = dto.Id;
        device.PresetId = invisiblePreset.Id;

        _context.Overclocking.Add(dto);

        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId)
    {
        return _context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetId, d => flightSheetId)
                                          .SetProperty(device => device.FlightSheetIsConfirm, d => false));
    }

    /// <inheritdoc/>
    public Task RemoveFlightSheet(Guid[] devicesIds)
    {
        return _context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetId, d => null)
                                          .SetProperty(device => device.FlightSheetIsConfirm, d => false));
    }

    /// <inheritdoc/>
    public Task ConfirmFlightSheet(Guid[] devicesIds)
    {
        return _context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetIsConfirm, d => true));
    }

    /// <inheritdoc/>
    public async Task<IOverclocking?> GetOverclocking(Guid deviceId, Guid userId)
    {
        var query = from device in _context.MiningDevices
                    .AsNoTrackingWithIdentityResolution()
                    .Available(new Specification(userId))
                    .Where(x => x.Id == deviceId)
                    join preset in _context.Presets.AsNoTracking()
                        on device.PresetId equals preset.Id
                    join overclocking in _context.Overclocking.AsNoTracking()
                        on preset.OverclockingId equals overclocking.Id
                    select overclocking;

        var clock = await query.FirstOrDefaultAsync();
        return _mapper.Map<IOverclocking>(clock);
    }

    /// <summary>
    /// Получить запрос списка устройств.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрос списка устройств. </returns>
    private IQueryable<MiningDeviceInfo> GetDevicesQuery(Specification specification)
    {
        return from device in _context.MiningDevices.AsNoTrackingWithIdentityResolution()
                                                    .Available(specification)
                                                    .Filter(specification)

               join flightSheet in _context.FlightSheets.AsNoTrackingWithIdentityResolution()
                                                        .Include(x => x.Targets)
                                                            .ThenInclude(target => target.Miner)
                                                        .Include(x => x.Targets)
                                                            .ThenInclude(target => target.CoinConfigs)

               on device.FlightSheetId equals flightSheet.Id into flightSheets

               from flightSheet in flightSheets.DefaultIfEmpty()
               select new MiningDeviceInfo()
               {
                   Id = device.Id,
                   Manufacturer = device.Manufacturer,
                   Model = device.Model,
                   OwnerId = device.OwnerId,
                   RigId = device.RigId,
                   LifeCycleStatus = device.LifeCycleStatus,
                   Type = device.Type,
                   FlightSheetId = device.FlightSheetId,
                   FlightSheetIsConfirm = device.FlightSheetIsConfirm,
                   FlightSheet = _mapper.Map<FlightSheet>(flightSheet)
               };
    }
}
