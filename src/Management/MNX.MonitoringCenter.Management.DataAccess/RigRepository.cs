using Polly;
using AutoMapper;
using System.Data;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.DataAccess.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Реализация <see cref="IRigRepository"/>.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly IMapper _mapper;

    private readonly ILogger<RigRepository> _logger;

    private readonly IDbContextFactory<Context> _contextFactory;

    public RigRepository(IMapper mapper,
                         ILogger<RigRepository> logger,
                         IDbContextFactory<Context> contextFactory)
    {
        _mapper = mapper
            ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger
            ?? throw new ArgumentNullException(nameof(logger));
        _contextFactory = contextFactory
            ?? throw new ArgumentNullException(nameof(contextFactory));
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(Guid id, Guid userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return await context.MiningDevices
                      .AsNoTracking()
                      .AnyAsync(x => x.RigId == id && x.OwnerId == userId);
    }

    /// <inheritdoc/>
    public async Task SetDevices(Guid rigId, List<(Core.Mining.MiningDevice.MiningDevice Devices, IOverclocking Overclockings)> devicesTuple)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

        await retryPolicy.ExecuteAsync(async () =>
        {
            using var context = _contextFactory.CreateDbContext();
            using var transaction = context.Database.BeginTransaction(IsolationLevel.RepeatableRead);

            try
            {
                // делаем выборку устройств всех ригов, для которых пришли устройства
                var dbDevices = await context.MiningDevices
                    .IgnoreQueryFilters()
                    .Where(device => device.RigId == rigId)
                    .ToListAsync();

                // берём ту часть устройств из БД, которая не пересекается со входящим набором устройств
                // ( те устройства, которые убрали с рига )
                // деактивируем их
                var noActiveDevices = dbDevices.ExceptBy(devicesTuple.Select(x => x.Devices.Id), device => device.Id).ToList();
                noActiveDevices.ForEach(device => device.Deactivate());

                // берём часть устройств из БД, которая пересекается со входящей коллекцией устройств
                // ставим статус "в сети" для полученных устройств
                var activeDevices = dbDevices.IntersectBy(devicesTuple.Select(x => x.Devices.Id), device => device.Id).ToList();
                activeDevices.ForEach(device => device.SwitchToOnline());

                await context.SaveChangesAsync();

                // берём часть из множества входящих устройств, которая не пересекается со множеством устройств из БД
                // обновляем их, если уже существуют в базе, иначе добавляем.
                var newDevices = devicesTuple
                    .Where(tuple => !dbDevices.Any(dbDevice => dbDevice.Id == tuple.Devices.Id))
                    .Select(tuple =>
                    {
                        var device = new MiningDeviceInfo()
                        {
                            Id = tuple.Devices.Id,
                            Manufacturer = tuple.Devices.Manufacturer,
                            Model = tuple.Devices.Model,
                            RigId = rigId,
                            OwnerId = tuple.Devices.OwnerId,
                            Type = tuple.Devices.Type,
                            FlightSheetConfirmationState = FlightSheetConfirmationState.Successfully
                        };

                        var overclocking = tuple.Overclockings;

                        return (device, overclocking);
                    }).ToList();

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
    public async Task SwitchToOffline(Guid rigId)
    {
        using var context = _contextFactory.CreateDbContext();
        await context.MiningDevices.Where(device => device.RigId == rigId).ExecuteUpdateAsync(x =>
            x.SetProperty(device => device.LifeCycleStatus, d => MiningDeviceLifeCycleStatus.Offline));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Guid> GetRigsByUserId(Guid userId)
    {
        using var context = _contextFactory.CreateDbContext();
        return context.MiningDevices
            .Where(md => md.OwnerId == userId)
            .Select(md => md.RigId!.Value)
            .AsAsyncEnumerable();
    }

    /// <summary>
    /// Добавить или обновить устройства.
    /// </summary>
    /// <param name="devices"> Устройства. </param>
    private async Task AddOrUpdateDevices(List<(MiningDeviceInfo Devices, IOverclocking Overclockings)> devices, Context context)
    {
        var dbDevices = (await context.MiningDevices
                                      .IgnoreQueryFilters()
                                      .Where(device => devices.Select(x => x.Devices.Id).Contains(device.Id))
                                      .ToListAsync())
                                      .ToHashSet();

        foreach (var (device, overclocking) in devices)
        {
            if (dbDevices.TryGetValue(device, out MiningDeviceInfo? dbDevice))
            {
                // По соглашению, для invisiblePreset наименование равно идентификатору девайса, к которому привязан пресет.
                var invisiblePreset = await context.Presets.FirstAsync(x => x.Name == device.Id.ToString());
                invisiblePreset.OverclockingId = overclocking.Id;

                dbDevice!.RigId = device.RigId;
                dbDevice.OwnerId = device.OwnerId;
                dbDevice.FlightSheetId = device.FlightSheetId;
                dbDevice.FlightSheetConfirmationState = device.FlightSheetConfirmationState;
                dbDevice.PresetId = invisiblePreset.Id;
                dbDevice.SwitchToOnline();

                await AddOverclocking(overclocking);
                await context.SaveChangesAsync();
            }
            else
            {
                var preset = new Core.Overclocking.Preset()
                {
                    Name = device.Id.ToString(),
                    DeviceName = device.Name,
                    UserId = device.OwnerId!.Value,
                    OverclockingId = overclocking.Id,
                    Overclocking = overclocking
                };
                await AddOverclocking(preset.Overclocking);
                await context.Presets.AddAsync(preset);
                await context.SaveChangesAsync();

                device.PresetId = preset.Id;
                device.Preset = preset;
                await context.MiningDevices.AddAsync(device);
                await context.SaveChangesAsync();
            }
        }

        async Task AddOverclocking(IOverclocking overclocking)
        {
            var overclockingDto = _mapper.Map<OverclockingDto>(overclocking);
            await context.Overclocking.AddAsync(overclockingDto);
        }
    }
}