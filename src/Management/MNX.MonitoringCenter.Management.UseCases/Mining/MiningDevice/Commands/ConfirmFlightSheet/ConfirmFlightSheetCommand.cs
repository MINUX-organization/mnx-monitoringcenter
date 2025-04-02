using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

/// <summary>
/// Подтвердить полётный лист на майнинг устройствах.
/// </summary>
/// <param name="SuccessfullyMiningDevicesIds">
/// Идентификаторы майнинг устройств, на которые
/// было подтверждено применение полетного листа.
/// </param>
/// <param name="SuccessfullyMiningDevicesIds">
/// Идентификаторы майнинг устройств, на которые
/// не было подтверждено применение полетного листа.
/// </param>
public sealed record ConfirmFlightSheetCommand(Guid[] SuccessfullyMiningDevicesIds,
                                               Guid[] UnsuccessfullyMiningDevicesIds)
    : IValidatableCommand<Unit>;

/// <summary>
/// Обработчик <see cref="ConfirmFlightSheetCommand"/>.
/// </summary>
public class ConfirmFlightSheetCommandHandler : IRequestHandler<ConfirmFlightSheetCommand, Result<Unit>>
{
    private readonly IMediator _mediator;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ConfirmFlightSheetCommandHandler(IMediator mediator,
                                            IMiningDeviceRepository miningDeviceRepository)
    {
        _mediator = mediator
            ?? throw new ArgumentNullException(nameof(mediator));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Unit>> Handle(ConfirmFlightSheetCommand request,
                                           CancellationToken cancellationToken)
    {
        await _miningDeviceRepository.ConfirmFlightSheet(request.SuccessfullyMiningDevicesIds);

        if (request.UnsuccessfullyMiningDevicesIds.Length != 0)
        {
            await _miningDeviceRepository
                .SetFlightSheetConfirmationStateToError(request.UnsuccessfullyMiningDevicesIds);
        }

        var someDevice = await _miningDeviceRepository.GetById(request.SuccessfullyMiningDevicesIds[0],
                                                               cancellationToken);
        var userId = someDevice.OwnerId;

        var devicesChangedStateEvent = new MiningDeviceStateChangedEvent(userId.ToString()!);
        await _mediator.Publish(devicesChangedStateEvent,
                                cancellationToken);

        return Result<Unit>.Empty();
    }
}
