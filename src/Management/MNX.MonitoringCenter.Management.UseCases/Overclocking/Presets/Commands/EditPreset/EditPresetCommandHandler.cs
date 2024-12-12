using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class EditPresetCommandHandler :
    SaveOverclockingBaseHandler,
    IRequestHandler<EditPresetCommand, Result<PresetModel>>
{
    private readonly IMapper _mapper;

    private readonly IPresetRepository _repository;

    public EditPresetCommandHandler(IMapper mapper,
                                    IMediator mediator,
                                    IPresetRepository repository)
        : base(mediator)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<PresetModel>> Handle(EditPresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (preset is null)
        {
            return Result<PresetModel>.Invalid("Preset with this Id wasn`t found");
        }

        var newPreset = _mapper.Map<Preset>(request);
        newPreset.DeviceName = preset.DeviceName;

        var overclockingValidationResult = await IsValidOverclocking(preset.DeviceName,
                                                                     preset.Overclocking!,
                                                                     cancellationToken);
        if (!overclockingValidationResult.IsSuccess)
        {
            return Result<PresetModel>.Invalid(overclockingValidationResult.Errors ?? new string[] { });
        }  

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
