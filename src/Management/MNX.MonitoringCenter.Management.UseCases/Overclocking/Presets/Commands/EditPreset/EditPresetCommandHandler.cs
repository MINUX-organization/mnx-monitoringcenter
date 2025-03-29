using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class EditPresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<EditPresetCommand, Result<PresetModel>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public EditPresetCommandHandler(IMapper mapper,
                                    IMediator mediator,
                                    IPresetRepository repository,
                                    IMiningDeviceRepository miningDeviceRepository)
        : base(mapper, mediator)
    {
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
                .Invalid($"Preset with id equaled {request.Id} wasn`t found");
        }

        var devices = await _miningDeviceRepository
            .GetAvailableByPresetId(request.Id, request.UserId);

        var newPreset = Map(preset, request);

        var overclockingValidationResult = await IsValidOverclocking(
            preset.DeviceName, newPreset.Overclocking!, cancellationToken);

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

        await _mediator.Publish(new SendOverclockingToRigsEvent(newPreset.Overclocking!,
                                                                devices,
                                                                request.UserId));

        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
    }

    /// <summary>
    /// Пользовательский маппер пресета для команды редактирования.
    /// </summary>
    /// <param name="preset"> Пресет. </param>
    /// <param name="command"> Команда. </param>
    /// <returns></returns>
    private Preset Map(Preset preset, EditPresetCommand command)
    {
        var newPreset = new Preset()
        {
            Id = command.Id,
            Name = command.Model.Name,
            DeviceName = preset.DeviceName,
            UserId = command.UserId,
            OverclockingId = preset.OverclockingId,
            IsVisible = true,
        };

        var overclocking = (IOverclocking)preset.Overclocking!.Clone();
        _mapper.Map(command.Model.Overclocking, overclocking);
        newPreset.Overclocking = overclocking;

        return newPreset;
    }
}
