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
        _context = context ?? throw new ArgumentNullException(nameof(context));
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
        return await _context.MiningDevices
            .AsNoTracking()
            .Where(x => x.OwnerId == userId && x.PresetId == presetId)
            .ToListAsync();
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
    public Task SetPreset(Guid presetId,
                          CancellationToken cancellationToken,
                          params Guid[] deviceIds)
    {
        return _context.MiningDevices.Where(x => deviceIds.Contains(x.Id))
            .ExecuteUpdateAsync(x => x.SetProperty(d => d.PresetId, presetId), cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetOverclocking(MiningDeviceInfo device,
                                      IOverclocking overclocking,
                                      CancellationToken cancellationToken)
    {
        var preset = await _context.Presets.AsNoTracking()
            .Where(x => x.Id == device.PresetId).FirstAsync(cancellationToken);

        var overclockingDto = _mapper.Map<OverclockingDto>(overclocking);

        if (preset!.IsVisible)
            await SetOverclockingFromVisiblePreset(device, overclockingDto, cancellationToken);
        else
            await UpdateOverclocking(preset.OverclockingId, overclockingDto, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
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
    /// Редактировать разгон.
    /// </summary>
    /// <param name="overclockingId"> Идентификатор разгона. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task UpdateOverclocking(Guid overclockingId,
                                          OverclockingDto overclocking,
                                          CancellationToken cancellationToken)
    {
        var overclockingToUpdate = await _context.Overclocking
            .AsNoTracking()
            .Where(x => x.Id == overclockingId)
            .FirstAsync(cancellationToken);

        _mapper.Map(overclocking, overclockingToUpdate);
        _context.Overclocking.Update(overclockingToUpdate);
    }

    /// <summary>
    /// Задать разгон из пользовательского пресета.
    /// </summary>
    /// <param name="device"> Майнинг-устройство. </param>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task SetOverclockingFromVisiblePreset(MiningDeviceInfo device,
                                                        OverclockingDto overclocking,
                                                        CancellationToken cancellationToken)
    {
        var invisiblePreset = await _context.Presets
            .AsNoTracking()
            .Where(x => x.Name == device.Id.ToString() && !x.IsVisible)
            .FirstAsync(cancellationToken);

        await UpdateOverclocking(invisiblePreset.OverclockingId, overclocking, cancellationToken);

        device.PresetId = invisiblePreset.Id;
        _context.MiningDevices.Update(device);
    }

    /// <summary>
    /// Получить запрос списка устройств.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Запрос списка устройств. </returns>
    private IQueryable<MiningDeviceInfo> GetDevicesQuery(Specification specification)
    {
        return from device in _context.MiningDevices.AsNoTracking()
                                                    .Available(specification)
                                                    .Filter(specification)

               // Присоединение полетных листов.
               join flightSheet in _context.FlightSheets.AsNoTracking()
                                                        .Include(x => x.Targets)
                                                            .ThenInclude(target => target.Miner)
                                                        .Include(x => x.Targets)
                                                            .ThenInclude(target => target.CoinConfigs)

               on device.FlightSheetId equals flightSheet.Id into flightSheets

               from flightSheet in flightSheets.DefaultIfEmpty()


               // Присоединение пресетов.
               join preset in _context.Presets.AsNoTracking()
                                          .Where(p => p.IsVisible)

               on device.PresetId equals preset.Id into presets

               from preset in presets.DefaultIfEmpty()

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
                   FlightSheet = _mapper.Map<FlightSheet>(flightSheet),
                   PresetId = device.PresetId,
                   Preset = preset
               };
    }
}
