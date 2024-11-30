using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Presets;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class EditPresetCommandHandler :
    SavePresetCommandBaseHandler,
    IRequestHandler<EditPresetCommand, Result<PresetModel>>
{
    public EditPresetCommandHandler(IMapper mapper,
                                    IMediator mediator,
                                    IPresetRepository repository)
        : base(mapper, mediator, repository) { }

    public async Task<Result<PresetModel>> Handle(EditPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (preset is null)
        {
            return Result<PresetModel>.Invalid("Preset with this Id wasn`t found");
        }

        var overclockingValidationResult = await IsValidOverclocking(preset.GpuName,
                                                                     request.Model.Overclocking,
                                                                     cancellationToken);
        if (!overclockingValidationResult.IsSuccess)
        {
            return overclockingValidationResult;
        }

        var newPreset = _mapper.Map<Preset>(request);
        newPreset.GpuName = preset.GpuName;

        if (preset.Equals(newPreset))
        {
            return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
        }

        if (preset.Name != newPreset.Name &&
            await _repository.Exists(request.UserId, request.Model.Name, cancellationToken))
        {
            return Result<PresetModel>.Conflict($"Preset with name equaled {request.Model.Name} already exists");
        }

        await _repository.Update(newPreset);

        return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
    }
}
