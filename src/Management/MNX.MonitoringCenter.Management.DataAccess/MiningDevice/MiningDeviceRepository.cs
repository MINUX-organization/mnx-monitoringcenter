using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

using MiningDeviceDetails = Core.MiningDevice.MiningDeviceDetails;

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
    public Task<MiningDeviceDetails?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices.AsNoTracking()
                                     .Where(x => x.IsActive)
                                     .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetCurrentRigsDevices(List<Core.MiningDevice.MiningDevice> devices,
                                            CancellationToken cancellationToken)
    {
        // делаем выборку устройств всех ригов, для которых пришли устройства
        var dbDevices = await _context
            .MiningDevices
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
                                .Select(device => new MiningDeviceDetails()
                                {
                                    Id = device.Id,
                                    RigId = device.RigId,
                                    Type = device.Type,
                                    IsActive = true
                                });

        await _context.MiningDevices.AddRangeAsync(newDevices, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task DeactivateDevicesByRigId(Guid rigId, CancellationToken cancellationToken)
    {
        var devices = await _context.MiningDevices.Where(device => device.RigId == rigId)
                                                  .ToListAsync(cancellationToken);

        devices.ForEach(device => device.IsActive = false);

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId, CancellationToken cancellationToken)
    {
        var devices = _context.MiningDevices.Where(device => devicesIds.Contains(device.Id))
                                            .AsAsyncEnumerable();

        await foreach (var device in devices)
        {
            device.FLightSheetId = flightSheetId;
        }

        await _context.SaveChangesAsync(cancellationToken);
    }
}
