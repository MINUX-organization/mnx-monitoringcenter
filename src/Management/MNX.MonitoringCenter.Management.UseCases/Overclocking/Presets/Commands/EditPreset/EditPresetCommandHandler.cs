using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class EditPresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<EditPresetCommand, Result<PresetModel>>
{
    private readonly IQueueBusClient _queueClient;

    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public EditPresetCommandHandler(IMapper mapper,
                                    IMediator mediator,
                                    IQueueBusClient queueClient,
                                    IPresetRepository repository,
                                    IMiningDeviceRepository miningDeviceRepository)
        : base(mapper, mediator)
    {
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
        _presetRepository = repository ??
            throw new ArgumentNullException(nameof(repository));
        _miningDeviceRepository = miningDeviceRepository ??
            throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<PresetModel>> Handle(EditPresetCommand request,
                                                  CancellationToken cancellationToken)
    {
        var preset = await _presetRepository.GetAvailableById(request.Id,
                                                        request.UserId,
                                                        cancellationToken);

        if (preset is null)
        {
            return Result<PresetModel>
                .Invalid("Preset with this Id wasn`t found");
        }

        var devices = await _miningDeviceRepository.GetAvailableByPresetId(request.Id, request.UserId);

        var newPreset = _mapper.Map<Preset>(request);
        newPreset.OverclockingId = preset.OverclockingId;
        newPreset.DeviceName = preset.DeviceName;
        newPreset.IsVisible = true;

        var overclockingValidationResult =
            await IsValidOverclocking(preset.DeviceName,
                                      newPreset.Overclocking!,
                                      cancellationToken);

        if (!overclockingValidationResult.IsSuccess)
        {
            return Result<PresetModel>.Invalid(
                overclockingValidationResult.Errors ?? new string[] { });
        }  

        if (preset.Equals(newPreset))
        {
            return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
        }

        if (preset.Name != newPreset.Name &&
            await _presetRepository.Exists(request.UserId,
                                     request.Model.Name,
                                     cancellationToken))
        {
            return Result<PresetModel>.Conflict(
                $"Preset with name equaled {request.Model.Name} already exists");
        }

        await _presetRepository.Update(newPreset);

        await SendOverclockingToRigs(newPreset.Overclocking!, devices, request.UserId);

        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
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
