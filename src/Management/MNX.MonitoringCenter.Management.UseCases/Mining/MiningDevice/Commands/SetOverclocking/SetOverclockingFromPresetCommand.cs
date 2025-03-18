using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using System.Transactions;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда на установку разгона на майнинг устройство через пресет.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="PresetId"> Идентификатор пресета. </param>
/// <param name="DeviceIds"> Идентификатор майнинг устройства. </param>
public sealed record SetOverclockingFromPresetCommand(Guid UserId,
                                                      Guid PresetId,
                                                      params Guid[] DeviceIds)
    : IUserableValidatableCommand<Guid[]>;


/// <summary>
/// Обработчик <see cref="SetOverclockingFromPresetCommand"/>.
/// </summary>
public class SetOverclockingFromPresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<SetOverclockingFromPresetCommand, Result<Guid[]>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    private readonly IQueueBusClient _queueClient;

    public SetOverclockingFromPresetCommandHandler(IPresetRepository repository,
                                                   IMediator mediator,
                                                   IMapper mapper,
                                                   IQueueBusClient queueClient,
                                                   IMiningDeviceRepository miningDeviceRepository)
        : base (mapper, mediator)
    {
        _presetRepository = repository ??
            throw new ArgumentNullException(nameof(repository));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
    }

    public async Task<Result<Guid[]>> Handle(SetOverclockingFromPresetCommand request,
                                           CancellationToken cancellationToken)
    {
        var devicesToProcess = new List<MiningDeviceInfo>(request.DeviceIds.Length);

        var errors = new List<string>();

        var preset = await _presetRepository.GetAvailableById(request.PresetId,
                                                              request.UserId,
                                                              cancellationToken);

        if (preset is null)
        {
            return Result<Guid[]>.Invalid(
                $"Preset with id equaled {request.PresetId} was not found");
        }

        foreach (var deviceId in request.DeviceIds)
        {
            var device = await _miningDeviceRepository.GetActiveDeviceById(deviceId,
                                                                           request.UserId,
                                                                           cancellationToken);

            if (device is null)
            {
                errors.Add($"Mining device with id equaled {deviceId} was not found");
                continue;
            }

            if (device.Type.ToString() != preset.Overclocking!.TargetDeviceType.ToString())
            {
                errors.Add($"Device with type of {device.Type} is not supported this overclocking");
                continue;
            }

            if (device.Name != preset.DeviceName)
            {
                errors.Add($"Incorrect device name of preset with id equaled {preset.Id}");
                continue;
            }

            devicesToProcess.Add(device);
        }

        if (devicesToProcess.Count == 0)
        {
            return Result<Guid[]>.Invalid(errors);
        }

        using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {
            await _miningDeviceRepository.SetPreset(request.PresetId,
                                                    cancellationToken,
                                                    devicesToProcess.Select(d => d.Id).ToArray());

            await SendOverclockingToRigs(preset.Overclocking!, devicesToProcess, request.UserId);

            transaction.Complete();
        }

        return Result<Guid[]>.Success(devicesToProcess.Select(x => x.Id).ToArray());
    }

    /// <summary>
    /// Отправить новый разгон на риги.
    /// </summary>
    /// <param name="overclocking"> Разгон. </param>
    /// <param name="devices"> Устройства. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    protected async Task SendOverclockingToRigs(IOverclocking overclocking, List<MiningDeviceInfo> devices, Guid userId)
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