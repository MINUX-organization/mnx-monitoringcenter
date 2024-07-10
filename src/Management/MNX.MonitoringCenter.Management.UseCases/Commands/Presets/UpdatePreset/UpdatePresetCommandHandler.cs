using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class UpdatePresetCommandHandler : IRequestHandler<UpdatePresetCommand, Result<PresetModel>>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public UpdatePresetCommandHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PresetModel>> Handle(UpdatePresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId).ConfigureAwait(false);

        if (preset == null)
        {
            return Result<PresetModel>.Invalid("Preset with this Id wasn`t found");
        }

        if (await _repository.Exists(request.UserId, request.SavePresetModel.Name, 
                                     request.SavePresetModel.GpuName, request.Id))
        {
            return Result<PresetModel>.Conflict("Preset already exists");
        }

        var newPreset = _mapper.Map<Preset>(request);
        await _repository.Update(newPreset).ConfigureAwait(false);

        return Result<PresetModel>.Empty();
    }
}
