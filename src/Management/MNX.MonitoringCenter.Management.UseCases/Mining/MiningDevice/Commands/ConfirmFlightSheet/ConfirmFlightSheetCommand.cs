using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

/// <summary>
/// Подтвердить полётный лист на майнинг устройствах.
/// </summary>
/// <param name="MiningDevices"> Идентификаторы майнинг устройств. </param>
public sealed record ConfirmFlightSheetCommand(Guid[] MiningDevices)
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
        if (request.MiningDevices.Length == 0)
            return Result<Unit>.Empty();

        await _miningDeviceRepository.ConfirmFlightSheet(request.MiningDevices);

        var someDevice = await _miningDeviceRepository.GetById(request.MiningDevices[0],
                                                               cancellationToken);
        var userId = someDevice.OwnerId;

        await _mediator.Publish(new MiningDeviceStateChangedEvent(userId.ToString()!), cancellationToken);

        return Result<Unit>.Empty();
    }
}
