using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using Polly;
using System.Data;

namespace MNX.MonitoringCenter.Management.DataAccess.MiningDevice;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;
using MiningDevice = Core.Mining.MiningDevice.MiningDevice;

/// <summary>
/// Реализация <see cref="IMiningDeviceRepository"/>.
/// </summary>
public class MiningDeviceRepository : IMiningDeviceRepository
{
    private readonly IMapper _mapper;

    private readonly ILogger<MiningDeviceRepository> _logger;

    private readonly IDbContextFactory<Context> _contextFactory;

    public MiningDeviceRepository(IDbContextFactory<Context> contextFactory,
                                  IMapper mapper,
                                  ILogger<MiningDeviceRepository> logger)
    {
        _contextFactory = contextFactory;
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<MiningDeviceInfo> GetAvailable(Specification specification)
    {
        return GetDevicesQuery(specification).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<MiningDeviceInfo?> GetActiveDeviceById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices.AsNoTracking()
                                    .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(string name, Guid userId, CancellationToken cancellationToken)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices
                      .AsNoTracking()
                      .Where(device => device.OwnerId == userId)
                      .AnyAsync(device => (device.Manufacturer + ' ' + device.Model) == name, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task SetCurrentRigsDevices(List<MiningDevice> devices)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await retryPolicy.ExecuteAsync(async () =>
        {
            var context = _contextFactory.CreateDbContext();
            using var transaction = context.Database.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                var groupedInputDevices = devices.GroupBy(x => x.RigId)
                                                 .ToDictionary(g => g.Key, g => g.ToList());

                // делаем выборку устройств всех ригов, для которых пришли устройства
                var dbDevices = await context.MiningDevices
                    .IgnoreQueryFilters()
                    .Where(device => groupedInputDevices.Keys.Contains(device.RigId))
                    .ToListAsync();

                // берём ту часть устройств из БД, которая не пересекается со входящим набором устройств
                // ( те устройства, которые убрали с рига )
                // деактивируем их
                var noActiveDevices = dbDevices.ExceptBy(devices.Select(x => x.Id), device => device.Id).ToList();
                noActiveDevices.ForEach(device => device.Deactivate());

                // берём часть устройств из БД, которая пересекается со входящей коллекцией устройств
                // ставим статус "в сети" для полученных устройств
                var activeDevices = dbDevices.IntersectBy(devices.Select(x => x.Id), device => device.Id).ToList();
                activeDevices.ForEach(device => device.SwitchToOnline());

                await context.SaveChangesAsync();

                // берём часть из множества входящих устройств, которая не пересекается со множеством устройств из БД
                // обновляем их, если уже существуют в базе, иначе добавляем.
                var newDevices = devices.ExceptBy(dbDevices.Select(x => x.Id), device => device.Id)
                                        .Select(device =>
                                        {
                                            var d = new MiningDeviceInfo()
                                            {
                                                Id = device.Id,
                                                Manufacturer = device.Manufacturer,
                                                Model = device.Model,
                                                RigId = device.RigId,
                                                OwnerId = device.OwnerId,
                                                Type = device.Type
                                            };

                                            d.SetOverclocking(device.Overclocking!);

                                            return d;
                                        })
                                        .ToList();

                await AddOrUpdateDevices(newDevices, context);
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError("Error of setting current rig devices: {error}", ex.Message);
                throw;
            }
        });
    }

    /// <inheritdoc/>
    public Task SetStatusForRigDevices(Guid rigId, MiningDeviceLifeCycleStatus status)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices.Where(device => device.RigId == rigId).ExecuteUpdateAsync(x =>
            x.SetProperty(device => device.LifeCycleStatus, d => status));
    }

    /// <inheritdoc/>
    public Task SetFlightSheet(Guid[] devicesIds, Guid flightSheetId)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetId, d => flightSheetId)
                                          .SetProperty(device => device.FlightSheetIsConfirm, d => false));
    }

    /// <inheritdoc/>
    public Task RemoveFlightSheet(Guid[] devicesIds)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetId, d => null)
                                          .SetProperty(device => device.FlightSheetIsConfirm, d => false));
    }

    /// <inheritdoc/>
    public Task ConfirmFlightSheet(Guid[] devicesIds)
    {
        var context = _contextFactory.CreateDbContext();
        return context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.FlightSheetIsConfirm, d => true));
    }

    /// <inheritdoc/>
    public async Task SetOverclocking(IOverclocking overclocking, params Guid[] devicesIds)
    {
        var context = _contextFactory.CreateDbContext();

        // todo: как удалять ненужный разгон?
        var dto = _mapper.Map<OverclockingDto>(overclocking);

        await context.Overclocking.AddAsync(dto);
        await context.SaveChangesAsync();

        await context.MiningDevices
                .Where(device => devicesIds.Contains(device.Id))
                .ExecuteUpdateAsync(x => x.SetProperty(device => device.OverclockingId, d => overclocking.Id));
    }

    /// <summary>
    /// Добавить или обновить устройства.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    private async Task AddOrUpdateDevices(List<MiningDeviceInfo> devices, Context context)
    {
        var dbDevices = (await context.MiningDevices
                                      .IgnoreQueryFilters()
                                      .Where(device => devices.Select(x => x.Id).Contains(device.Id))
                                      .ToListAsync())
                                      .ToHashSet();

        foreach (var device in devices)
        {
            if (dbDevices.TryGetValue(device, out MiningDeviceInfo? dbDevice))
            {
                dbDevice.RigId = device.RigId;
                dbDevice.OwnerId = device.OwnerId;
                dbDevice.FlightSheetIsConfirm = device.FlightSheetIsConfirm;
                dbDevice.SwitchToOnline();

                await context.SaveChangesAsync();
            }
            else
            {
                var overclocking = _mapper.Map<OverclockingDto>(device.Overclocking);
                await context.Overclocking.AddAsync(overclocking);
                await context.SaveChangesAsync();

                await context.MiningDevices.AddAsync(device);
                await context.SaveChangesAsync();
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
        var context = _contextFactory.CreateDbContext();

        return from device in context.MiningDevices.AsNoTrackingWithIdentityResolution()
                                                   .Available(specification)
                                                   .Filter(specification)

               join flightSheet in context.FlightSheets.AsNoTrackingWithIdentityResolution()
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
                   FlightSheet = _mapper.Map<FlightSheet>(flightSheet),
                   OverclockingId = device.OverclockingId
               };
    }
}
