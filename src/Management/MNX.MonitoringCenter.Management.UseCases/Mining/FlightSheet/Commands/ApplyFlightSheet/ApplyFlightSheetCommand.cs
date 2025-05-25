using MediatR;
using MNX.Application.UseCases.Results;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.Application.UseCases.CommandValidation;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.Core.Mining.FlightSheet.Target;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.ApplyFlightSheet;

using FlightSheet = Core.Mining.FlightSheet.FlightSheet;

/// <summary>
/// Команда применения полётного листа на майнинг устройство.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="FlightSheetId"> Идентификатор полётного листа. </param>
/// <param name="MiningDevices"> Идентификаторы майнинг устройств. </param>
public sealed record ApplyFlightSheetCommand(Guid UserId, Guid FlightSheetId, Guid[] MiningDevices)
    : IValidatableCommand<IEnumerable<Guid>>;


/// <summary>  
/// Обработчик <see cref="ApplyFlightSheetCommand"/>.
/// </summary>
public class ApplyFlightSheetCommandHandler : IRequestHandler<ApplyFlightSheetCommand, Result<IEnumerable<Guid>>>
{
    private readonly IMediator _mediator;

    private readonly IQueueBusClient _bus;

    private readonly IFlightSheetRepository _flightSheetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ApplyFlightSheetCommandHandler(IMediator mediator,
                                          IQueueBusClient bus,
                                          IFlightSheetRepository flightSheetRepository,
                                          IMiningDeviceRepository miningDeviceRepository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        _bus = bus ?? throw new ArgumentNullException(nameof(bus));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<IEnumerable<Guid>>> Handle(ApplyFlightSheetCommand request, CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository
            .GetAvailableById(request.FlightSheetId, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<IEnumerable<Guid>>.Invalid("Flight sheet wasn`t found");
        }

        var (DevicesIdsToApply, DevicesIdsToDisapply, CurrentDevices) =
            await GetDevicesForProcessing(request, flightSheet, cancellationToken);

        await _miningDeviceRepository.RemoveFlightSheet(DevicesIdsToDisapply.Select(x => x.Id).ToArray());
        await _miningDeviceRepository.SetFlightSheet(DevicesIdsToApply.Select(x => x.Id).ToArray(), request.FlightSheetId);

        await SendMessagesToRigs(request.UserId, flightSheet, DevicesIdsToApply, DevicesIdsToDisapply, cancellationToken);

        var resultDevices = DevicesIdsToApply.ToList();
        resultDevices.AddRange(CurrentDevices);

        await InstallTargetMinersCommand(DevicesIdsToApply, flightSheet.Targets, request.UserId, cancellationToken);

        return Result<IEnumerable<Guid>>.Success(resultDevices.Select(x => x.Id));
    }

    /// <summary>
    /// Получить устройства для обработки.
    /// </summary>
    /// <param name="request"> Запрос на применение полётного листа. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// Кортеж из списков устройств на применение и на снятие полётного листа,
    /// а также устройства, на которых уже был применён данный полётный лист.
    /// </returns>
    private async Task<(List<MiningDeviceModel> DevicesIdsToApply,
                        List<MiningDeviceModel> DevicesIdsToDisapply,
                        List<MiningDeviceModel> CerrentDevicesIds)>
        GetDevicesForProcessing(ApplyFlightSheetCommand request, FlightSheet flightSheet, CancellationToken cancellationToken)
    {
        var availableToApplyDevicesQuery = new GetFlightSheetSupportedDevicesQuery(request.UserId, request.FlightSheetId);
        var availableToApplyDevices = _mediator.CreateStream(availableToApplyDevicesQuery, cancellationToken);

        var inputDevices = request.MiningDevices.ToHashSet();
        var devicesToApply = new List<MiningDeviceModel>(request.MiningDevices.Length);
        var devicesToDisapply = new List<MiningDeviceModel>(request.MiningDevices.Length);
        var currentDevices = new List<MiningDeviceModel>(request.MiningDevices.Length);

        await foreach (var device in availableToApplyDevices.WithCancellation(cancellationToken))
        {
            if (inputDevices.Contains(device.Id))
            {
                // если на устройстве не было полётного листа или был установлен другой полётный лист
                if (device.FlightSheetName == null || device.FlightSheetName != flightSheet.Name)
                {
                    devicesToApply.Add(device);
                }
                // на устройстве уже установлен данный полётный лист
                else
                {
                    currentDevices.Add(device);
                }
            }
            // если с устройства был снят полётный лист
            else if (device.FlightSheetName == flightSheet.Name)
            {
                devicesToDisapply.Add(device);
            }
        }

        return new(devicesToApply, devicesToDisapply, currentDevices);
    }

    /// <summary>
    /// Отправить сообщения ригам.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="flightSheet"> Полётный лист. </param>
    /// <param name="devicesToApply"> Устройства на применение полётного листа. </param>
    /// <param name="devicesToDisapply"> Устройства на снятие полётного листа. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task SendMessagesToRigs(Guid userId,
                                          FlightSheet flightSheet,
                                          List<MiningDeviceModel> devicesToApply,
                                          List<MiningDeviceModel> devicesToDisapply,
                                          CancellationToken cancellationToken)
    {
        var deviceFlightSheets = devicesToApply.Select(device => new DeviceFLightSheet(device, flightSheet)).ToList();
        deviceFlightSheets.AddRange(devicesToDisapply.Select(device => new DeviceFLightSheet(device)));
        var groupedDeviceFlightSheets = deviceFlightSheets.GroupBy(x => x.Device.RigId);

        foreach (var group in groupedDeviceFlightSheets)
        {
            await _mediator.Publish(new FlightSheetAppliedToRigEvent(group.Key, userId, group.ToList()), cancellationToken);
        }
    }
    
    /// <summary>
    /// Произвести поиск ригов, у которых отсутствует майнер
    /// и отправить команду на установку майнеров на риги.
    /// </summary>
    /// <param name="appliedDevices">
    /// Майнинг-устройства, к которым был применен полетный лист.
    /// </param>
    /// <param name="targets"> Таргеты полетного листа, в которых хранятся майнеры. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task InstallTargetMinersCommand(List<MiningDeviceModel> appliedDevices,
                                                  List<FlightSheetTarget> targets,
                                                  Guid userId,
                                                  CancellationToken cancellationToken)
    {
        await ProcessCheckoutRigs(
            appliedDevices, targets, userId, MiningDeviceType.CPU, cancellationToken);

        await ProcessCheckoutRigs(
            appliedDevices, targets, userId, MiningDeviceType.GPU, cancellationToken);
    }

    /// <summary>
    /// Обработать процесс поиска ригов без майнера и отправить команду на установку.
    /// </summary>
    /// <param name="devices"> Список девайсов. </param>
    /// <param name="targets"> Таргеты. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="deviceType"> Тип девайса для обработки. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task ProcessCheckoutRigs(List<MiningDeviceModel> devices,
                                           List<FlightSheetTarget> targets,
                                           Guid userId,
                                           MiningDeviceType deviceType,
                                           CancellationToken cancellationToken)
    {
        var rigIdsByType = devices.Where(x => x.Type == deviceType.ToString()).Select(x => x.RigId).Distinct().ToArray();
        if (rigIdsByType.Length == 0) return;

        var targetMiner = targets.FirstOrDefault(x => x.DeviceType == deviceType)?.Miner;
        if (targetMiner == null) return;

        var rigIdsForDevicesType = await _mediator.Send(new GetRigIdsWithoutMinerQuery(
                rigIdsByType, userId, targetMiner.Name, targetMiner.Version), cancellationToken);

        if (rigIdsForDevicesType.Length == 0) return;

        await _bus.Enqueue(new InstallMinerCommand(
                targetMiner.Name,
                targetMiner.Version,
                targetMiner.InstallationUrl,
                targetMiner.PoolTemplate,
                targetMiner.WalletWorkerTemplate,
                targetMiner.Type.ToString()),
            rigIdsForDevicesType,
            userId,
            cancellationToken: cancellationToken
        );
    }
}
