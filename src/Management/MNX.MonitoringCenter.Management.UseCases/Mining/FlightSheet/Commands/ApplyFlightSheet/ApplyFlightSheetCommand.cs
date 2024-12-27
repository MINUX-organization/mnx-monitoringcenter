using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;
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

    private readonly IFlightSheetRepository _flightSheetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ApplyFlightSheetCommandHandler(IMediator mediator,
                                          IFlightSheetRepository flightSheetRepository,
                                          IMiningDeviceRepository miningDeviceRepository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

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
                // если на устройстве не было полётного листа
                if (device.FlightSheetName == null)
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

        // todo: удалить, когда будет налажена связь с ригом.
        await Task.WhenAll(
            _mediator.Send(new ConfirmFlightSheetCommand(devicesToApply.Select(x => x.Id).ToArray()), cancellationToken),
            _mediator.Send(new ConfirmFlightSheetCommand(devicesToDisapply.Select(x => x.Id).ToArray()), cancellationToken)
        );
    }
}
