using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice;
using System.Data;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

using MiningDeviceInfo = Core.MiningDevice.MiningDeviceInfo;
using MiningDevice = Core.MiningDevice.MiningDevice;
using FlightSheet = Core.FlightSheet.FlightSheet;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/>.
/// </summary>
public class MiningDeviceRepository : IMiningDeviceRepository
{
    private readonly Context _context;

    private readonly IMapper _mapper;

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
    public Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.MiningDevices.AsNoTracking()
                                     .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MiningDeviceInfo> GetFlightSheetSupportedDevices(FlightSheet flightSheet)
    {
        return GetDevicesQuery(new Specification(flightSheet.UserId))
                .Where(device => flightSheet.IsDeviceSupport(device))
                .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task SetCurrentRigsDevices(List<MiningDevice> devices,
                                            CancellationToken cancellationToken)
    {
        using var transaction = _context.Database.BeginTransaction(IsolationLevel.ReadCommitted);

        try
        {
            var groupedDevices = devices.GroupBy(x => x.RigId)
                                        .ToDictionary(g => g.Key, g => g.ToList());

            // делаем выборку устройств всех ригов, для которых пришли устройства
            var dbDevices = await _context.MiningDevices
                .IgnoreQueryFilters()
                .Where(device => groupedDevices.Keys.Contains(device.RigId))
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
            // обновляем их, если уже существуют в базе, иначе добавляем.
            var newDevices = devices.ExceptBy(dbDevices.Select(x => x.Id), device => device.Id)
                                    .Select(device => new MiningDeviceInfo()
                                    {
                                        Id = device.Id,
                                        RigId = device.RigId,
                                        OwnerId = device.OwnerId,
                                        Type = device.Type,
                                        IsActive = true
                                    })
                                    .ToList();

            await AddOrUpdateDevices(newDevices, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception)
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
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

    /// <summary>
    /// Добавить или обновить устройства.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task AddOrUpdateDevices(List<MiningDeviceInfo> devices,
                                          CancellationToken cancellationToken)
    {
        var dbDevices = (await _context.MiningDevices
                                       .IgnoreQueryFilters()
                                       .Where(device => devices.Select(x => x.Id).Contains(device.Id))
                                       .ToListAsync(cancellationToken))
                                       .ToHashSet();

        foreach (var device in devices)
        {
            if (dbDevices.TryGetValue(device, out MiningDeviceInfo? dbDevice))
            {
                dbDevice.RigId = device.RigId;
                dbDevice.OwnerId = device.OwnerId;
                dbDevice.IsActive = device.IsActive;
            }
            else
            {
                await _context.MiningDevices.AddAsync(device, cancellationToken);
            }
        }
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
                    on device.FlightSheetId equals flightSheet.Id

               select new MiningDeviceInfo()
               {
                   Id = device.Id,
                   OwnerId = device.OwnerId,
                   RigId = device.RigId,
                   IsActive = device.IsActive,
                   Type = device.Type,
                   FlightSheetId = device.FlightSheetId,
                   FlightSheet = _mapper.Map<FlightSheet>(flightSheet)
               };
    }
}
