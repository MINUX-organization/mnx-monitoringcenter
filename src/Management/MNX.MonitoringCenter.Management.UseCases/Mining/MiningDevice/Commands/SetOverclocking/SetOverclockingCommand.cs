using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда на установку разгона на майнинг устройство.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="DeviceIds"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingCommand(Guid UserId, IOverclockingModel Overclocking, params Guid[] DeviceIds)
    : IUserableValidatableCommand<Guid[]>;


/// <summary>
/// Обработчик <see cref="SetOverclockingCommand"/>.
/// </summary>
public class SetOverclockingCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<SetOverclockingCommand, Result<Guid[]>>
{
    private readonly IMapper _mapper;

    private readonly ILogger<SetOverclockingCommandHandler> _logger;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public SetOverclockingCommandHandler(
        IMapper mapper,
        IMediator mediator,
        ILogger<SetOverclockingCommandHandler> logger,
        IMiningDeviceRepository miningDeviceRepository)
        : base(mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Guid[]>> Handle(SetOverclockingCommand request, CancellationToken cancellationToken)
    {
        var devicesToProcessing = new List<Guid>(request.DeviceIds.Length);
        var overclocking = _mapper.Map<IOverclocking>(request.Overclocking);

        foreach (var deviceId in request.DeviceIds)
        {
            var device = await _miningDeviceRepository.GetActiveDeviceById(deviceId, request.UserId, cancellationToken);

            if (device is null)
            {
                _logger.LogError("Mining device with id equaled {id} was not found!", deviceId);
                continue;
            }

            if (device.Type.ToString() != request.Overclocking.TargetDeviceType.ToString())
            {
                _logger.LogError("Device with type of {deviceType} is not supported this overclocking", device.Type.ToString());
                continue;
            }

            var overclockingValidationResult =
                await IsValidOverclocking(device.Name, overclocking, cancellationToken);

            if (!overclockingValidationResult.IsSuccess)
            {
                LogErrors(overclockingValidationResult.Errors ?? Array.Empty<string>());
                continue;
            }

            devicesToProcessing.Add(deviceId);
        }

        if (devicesToProcessing.Count == 0)
            return Result<Guid[]>.Success(devicesToProcessing.ToArray());

        await _miningDeviceRepository
            .SetOverclocking(overclocking, devicesToProcessing.ToArray());

        // todo: отправка сообщения на риги

        return Result<Guid[]>.Success(devicesToProcessing.ToArray());
    }

    private void LogErrors(IReadOnlyCollection<string> errors)
    {
        foreach (var error in errors)
        {
            _logger.LogError("{error}", error);
        }
    }
}