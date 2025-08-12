using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.ApplyPreset;

/// <summary>
/// Команда на установку разгона на майнинг устройство через пресет.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="PresetId"> Идентификатор пресета. </param>
/// <param name="DeviceIds"> Идентификатор майнинг устройства. </param>
public sealed record ApplyPresetCommand(Guid UserId, Guid PresetId, params Guid[] DeviceIds)
    : IUserableValidatableCommand<Guid[]>;


/// <summary>
/// Обработчик <see cref="ApplyPresetCommand"/>.
/// </summary>
public class ApplyPresetCommandHandler :
    IRequestHandler<ApplyPresetCommand, Result<Guid[]>>
{
    private readonly IMediator _mediator;

    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ApplyPresetCommandHandler(IMediator mediator,
                                     IPresetRepository repository,
                                     IMiningDeviceRepository miningDeviceRepository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _presetRepository = repository ??
            throw new ArgumentNullException(nameof(repository));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Guid[]>> Handle(ApplyPresetCommand request,
                                             CancellationToken cancellationToken)
    {
        var devicesToProcess = new List<MiningDeviceInfo>(request.DeviceIds.Length);

        var errors = new List<string>();

        var preset = await _presetRepository.GetById(request.PresetId,
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

            if (device.GetOverclockingType().ToString() != preset.Overclocking!.TargetDeviceType.ToString())
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

        await _miningDeviceRepository.SetPreset(request.PresetId,
                                                cancellationToken,
                                                devicesToProcess.Select(d => d.Id).ToArray());

        await _mediator.Publish(new SendOverclockingToRigsEvent(preset.Overclocking!,
                                                                devicesToProcess,
                                                                request.UserId),
                                                                cancellationToken);

        return Result<Guid[]>.Success(devicesToProcess.Select(x => x.Id).ToArray());
    }
}