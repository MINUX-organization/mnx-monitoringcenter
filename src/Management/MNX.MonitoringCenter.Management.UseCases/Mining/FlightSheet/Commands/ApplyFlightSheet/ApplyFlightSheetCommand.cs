using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
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

        await _miningDeviceRepository.RemoveFlightSheet(DevicesIdsToDisapply.ToArray());
        await _miningDeviceRepository.SetFlightSheet(DevicesIdsToApply.ToArray(), request.FlightSheetId);

        await SendMessagesToRigs(DevicesIdsToApply, DevicesIdsToDisapply, cancellationToken);

        var resultDevices = DevicesIdsToApply.ToList();
        resultDevices.AddRange(CurrentDevices);
        return Result<IEnumerable<Guid>>.Success(resultDevices);
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
    private async Task<(List<Guid> DevicesIdsToApply, List<Guid> DevicesIdsToDisapply, List<Guid> CerrentDevicesIds)>
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

        return new(
            devicesToApply.Select(device => device.Id).ToList(),
            devicesToDisapply.Select(device => device.Id).ToList(),
            currentDevices.Select(device => device.Id).ToList()
        );
    }

    /// <summary>
    /// Отправить сообщения ригам.
    /// </summary>
    /// <param name="devicesToApply"> Устройства на применение полётного листа. </param>
    /// <param name="devicesToDisapply"> Устройства на снятие полётного листа. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    private async Task SendMessagesToRigs(List<Guid> devicesToApply, List<Guid> devicesToDisapply, CancellationToken cancellationToken)
    {
        // todo: рассылка сообщений ригам.

        await Task.WhenAll(
            _mediator.Send(new ConfirmFlightSheetCommand(devicesToApply.ToArray()), cancellationToken),
            _mediator.Send(new ConfirmFlightSheetCommand(devicesToDisapply.ToArray()), cancellationToken)
        );
    }
}
