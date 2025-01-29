using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using MNX.Application.UseCases.Requests;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.RigCommander.MessageQueue.Clients.Bus;

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

    private readonly IQueueBusClient _queueClient;

    private readonly ILogger<SetOverclockingCommandHandler> _logger;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public SetOverclockingCommandHandler(
        IMapper mapper,
        IMediator mediator,
        IQueueBusClient queueClient,
        ILogger<SetOverclockingCommandHandler> logger,
        IMiningDeviceRepository miningDeviceRepository)
        : base(mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Guid[]>> Handle(SetOverclockingCommand request, CancellationToken cancellationToken)
    {
        var devicesToProcessing = new List<MiningDeviceInfo>(request.DeviceIds.Length);
        var overclocking = _mapper.Map<IOverclocking>(request.Overclocking);

        var errors = new List<string>();

        foreach (var deviceId in request.DeviceIds)
        {
            var device = await _miningDeviceRepository.GetActiveDeviceById(deviceId, request.UserId, cancellationToken);

            if (device is null)
            {
                errors.Add($"Mining device with id equaled {deviceId} was not found!");
                continue;
            }

            if (device.Type.ToString() != request.Overclocking.TargetDeviceType.ToString())
            {
                errors.Add($"Device with type of {device.Type} is not supported this overclocking");
                continue;
            }

            var overclockingValidationResult =
                await IsValidOverclocking(device.Name, overclocking, cancellationToken);

            if (!overclockingValidationResult.IsSuccess)
            {
                if (overclockingValidationResult.Errors is not null)
                {
                    errors.AddRange(overclockingValidationResult.Errors);
                }
                
                continue;
            }

            devicesToProcessing.Add(device);
        }

        if (devicesToProcessing.Count == 0)
        {
            return Result<Guid[]>.Invalid(errors);
        }

        await _miningDeviceRepository
            .SetOverclocking(overclocking, devicesToProcessing.Select(x => x.Id).ToArray());

        await SendOverclockingToRigs(overclocking, devicesToProcessing, request.UserId);

        return Result<Guid[]>.Success(devicesToProcessing.Select(x => x.Id).ToArray());
    }

    private async Task SendOverclockingToRigs(IOverclocking overclocking, List<MiningDeviceInfo> devices, Guid userId)
    {
        foreach (var rigDevices in devices.GroupBy(x => x.RigId))
        {
            var rigOverclocking = _mapper.Map<Inventory.Contracts.Devices.Overclocking>(overclocking);

            var command = new Agent.Commands.Overclocking.SetOverclockingCommand(
                rigOverclocking, rigDevices.Select(x => x.Id).ToArray());

            await _queueClient.Enqueue(command, new Guid[] { rigDevices.Key }, userId);
        }
    }
}