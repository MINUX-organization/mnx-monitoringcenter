using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

using MiningDeviceInfo = Core.MiningDevice.MiningDeviceInfo;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/>.
/// </summary>
public class MiningDeviceRepository : IMiningDeviceRepository
{
    private readonly Context _context;

    public MiningDeviceRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MiningDeviceInfo> GetAvailable(Specification specification)
    {
        return _context.MiningDevices.AsNoTrackingWithIdentityResolution()
                                     .Include(device => device.FlightSheet)
                                     .Available(specification)
                                     .Filter(specification)
                                     .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices.AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetCurrentRigsDevices(List<Core.MiningDevice.MiningDevice> devices,
                                            CancellationToken cancellationToken)
    {
        // делаем выборку устройств всех ригов, для которых пришли устройства
        var dbDevices = await _context
            .MiningDevices
            .IgnoreQueryFilters()
            .Where(device => devices.ToDictionary(x => x.RigId).Keys.Contains(device.RigId))
            .ToListAsync(cancellationToken);

        // берём ту часть устройств из БД, которая не пересекается со входящим набором устройств
        // ( те устройства, которые убрали с рига )
        // для них мы ставим признак не активности
        var noActiveDevices = dbDevices.ExceptBy(devices.Select(x => x.Id), device => device.Id).ToList();
        noActiveDevices.ForEach(device => device.IsActive = false);

        // берём часть устройств из БД, которая пересекается со входящей коллекцией устройств
        // активируем полученные устройства
        var activeDevices = dbDevices.IntersectBy(devices.Select(x => x.Id), device => device.Id).ToList();
        activeDevices.ForEach(device => device.IsActive = true);

        // берём часть из множества входящих устройств, которая не пересекается со множеством устройств из БД
        // записываем их в базу
        var newDevices = devices.ExceptBy(dbDevices.Select(x => x.Id), device => device.Id)
                                .Select(device => new MiningDeviceInfo()
                                {
                                    Id = device.Id,
                                    RigId = device.RigId,
                                    OwnerId = device.OwnerId,
                                    Type = device.Type,
                                    IsActive = true
                                });

        await _context.MiningDevices.AddRangeAsync(newDevices, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task DeactivateDevicesByRigId(Guid rigId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices.Where(device => device.RigId == rigId).ExecuteUpdateAsync(x =>
            x.SetProperty(device => device.IsActive, d => false), cancellationToken);
    }

    /// <inheritdoc/>
    public Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices.Where(device => devicesIds.Contains(device.Id)).ExecuteUpdateAsync(x =>
            x.SetProperty(device => device.FlightSheetId, d => flightSheetId), cancellationToken);
    }
}
