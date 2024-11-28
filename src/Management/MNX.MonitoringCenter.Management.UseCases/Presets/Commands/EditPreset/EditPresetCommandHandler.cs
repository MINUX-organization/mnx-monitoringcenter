using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class EditPresetCommandHandler : IRequestHandler<EditPresetCommand, Result<PresetModel>>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public EditPresetCommandHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(EditPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (preset is null)
        {
            return Result<PresetModel>.Invalid("Preset with this Id wasn`t found");
        }

        var newPreset = _mapper.Map<Preset>(request);

        if (preset.Equals(newPreset))
        {
            return Result<PresetModel>.Success(_mapper.Map<PresetModel>(newPreset));
        }

        if (preset.GpuName != preset.GpuName)
        {
            return Result<PresetModel>.Invalid("Cannot change the gpu name");
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
