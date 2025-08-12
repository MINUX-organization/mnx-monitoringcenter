using MediatR;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда на установку разгона на майнинг устройство.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="DeviceId"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingCommand(Guid UserId, IOverclockingModel Overclocking, Guid DeviceId)
    : IUserableValidatableCommand<Guid>;

/// <summary>
/// Обработчик <see cref="SetOverclockingCommand"/>.
/// </summary>
public class SetOverclockingCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<SetOverclockingCommand, Result<Guid>>
{
    private readonly IOverclockingModelMapper<IOverclockingModel, IOverclocking> _overclockingMapper;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    ///
    public SetOverclockingCommandHandler(IMediator mediator,
                                         IMiningDeviceRepository miningDeviceRepository,
                                         IPresetRepository presetRepository,
                                         IOverclockingModelMapper<IOverclockingModel, IOverclocking> overclockingMapper)
        : base(mediator)
    {
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
        _overclockingMapper = overclockingMapper ??
            throw new ArgumentNullException(nameof(overclockingMapper));
    }

    ///
    public async Task<Result<Guid>> Handle(SetOverclockingCommand request,
                                           CancellationToken cancellationToken)
    {
        var device = await _miningDeviceRepository.GetActiveDeviceById(request.DeviceId, request.UserId, cancellationToken);

        if (device is null)
        {
            return Result<Guid>.Invalid($"Mining device with id equaled {request.DeviceId} was not found");
        }

        if (device.GetOverclockingType().ToString() != request.Overclocking.TargetDeviceType.ToString())
        {
            return Result<Guid>.Invalid($"Device with type of {device.Type} is not supported this overclocking");
        }

        var overclocking = _overclockingMapper.MapToCoreEntity(request.Overclocking);

        var overclockingValidationResult =
                await ValidateOverclocking(device.Id, overclocking, cancellationToken);

        if (!overclockingValidationResult.IsSuccess)
        {
            if (overclockingValidationResult.Errors is not null)
            {
                return Result<Guid>.Invalid(overclockingValidationResult.Errors);
            }
        }

        await _miningDeviceRepository.SetOverclocking(device,
                                                      overclocking,
                                                      cancellationToken);

        await _mediator.Publish(new SendOverclockingToRigsEvent(overclocking,
                                                               [device],
                                                               request.UserId),
                                                               cancellationToken);

        return Result<Guid>.Success(device.Id);
    }
}