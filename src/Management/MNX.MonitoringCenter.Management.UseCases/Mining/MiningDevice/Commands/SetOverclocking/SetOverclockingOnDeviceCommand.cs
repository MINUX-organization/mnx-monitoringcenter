using MediatR;
using AutoMapper;
using EasyNetQ.Logging;
using System.Transactions;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда на установку разгона на майнинг устройство.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="DeviceId"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingOnDeviceCommand(Guid UserId,
                                                    IOverclockingModel Overclocking,
                                                    Guid DeviceId)
    : IUserableValidatableCommand<Guid>;

/// <summary>
/// Обработчик <see cref="SetOverclockingOnDeviceCommand"/>.
/// </summary>
public class SetOverclockingOnDeviceCommandHandler
    : SaveOverclockingBaseHandler,
    IRequestHandler<SetOverclockingOnDeviceCommand, Result<Guid>>
{
    private readonly ILogger<SetOverclockingOnDeviceCommandHandler> _logger;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    private readonly IPresetRepository _presetRepository;

    private readonly IQueueBusClient _queueClient;

    public SetOverclockingOnDeviceCommandHandler(IMapper mapper,
                                                 IMediator mediator,
                                                 IQueueBusClient queueClient,
                                                 ILogger<SetOverclockingOnDeviceCommandHandler> logger,
                                                 IMiningDeviceRepository miningDeviceRepository,
                                                 IPresetRepository presetRepository)
        : base(mapper, mediator)
    {
        _logger = logger ??
            throw new ArgumentNullException(nameof(logger));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
        _presetRepository = presetRepository ??
            throw new ArgumentNullException(nameof(presetRepository));
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
    }

    public async Task<Result<Guid>> Handle(SetOverclockingOnDeviceCommand request,
                                       CancellationToken cancellationToken)
    {
        var device = await _miningDeviceRepository.GetActiveDeviceById(request.DeviceId, request.UserId, cancellationToken);

        if (device is null)
        {
            return Result<Guid>.Error($"Mining device with id equaled {request.DeviceId} was not found");
        }

        if (device.Type.ToString() != request.Overclocking.TargetDeviceType.ToString())
        {
            return Result<Guid>.Error($"Device with type of {device.Type} is not supported this overclocking");
        }

        var overclocking = _mapper.Map<IOverclocking>(request.Overclocking);

        var overclockingValidationResult =
                await IsValidOverclocking(device.Name, overclocking, cancellationToken);

        if (!overclockingValidationResult.IsSuccess)
        {
            if (overclockingValidationResult.Errors is not null)
            {
                return Result<Guid>.Error(overclockingValidationResult.Errors);
            }
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _miningDeviceRepository.SetOverclocking(device,
                                                          overclocking);

            await SendOverclockingToRigs(overclocking, device, request.UserId);

            transaction.Complete();
        }

        return Result<Guid>.Success(device.Id);
    }

    /// <summary>
    /// Отправить новый разгон на риги.
    /// </summary>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="device"> Устройство. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    protected async Task SendOverclockingToRigs(IOverclocking overclocking, MiningDeviceInfo device, Guid userId)
    {
        var rigOverclocking = _mapper.Map<Inventory.Contracts.Devices.Overclocking>(overclocking);

        var command = new Agent.Commands.Overclocking.SetOverclockingCommand(
            rigOverclocking, device.Id);

        await _queueClient.Enqueue(command, [device.RigId], userId);
    }
}