using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Обработчик команды сохранения пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommandHandler : IRequestHandler<SavePresetCommand, Result<PresetModel>>
{
    private readonly IPresetRepository _presetRepository;

    private readonly IMapper _mapper;

    public SavePresetCommandHandler(IPresetRepository repository,
                                    IMapper mapper)
    {
        _presetRepository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(SavePresetCommand request, CancellationToken cancellationToken)
    {
        if (await _presetRepository.Exists(request.UserId, request.Model.Name))
        {
            return Result<PresetModel>.Conflict("Preset already exists");
        }

        // todo: gpu exists?

        var preset = _mapper.Map<Preset>(request);
        await _presetRepository.Save(preset);

        return Result<PresetModel>.SuccessfullyCreated(_mapper.Map<PresetModel>(preset));
    }
}
