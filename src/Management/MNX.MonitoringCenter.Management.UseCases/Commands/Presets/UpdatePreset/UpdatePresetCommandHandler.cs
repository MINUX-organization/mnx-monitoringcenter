using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Обработчик команды редактирования пресета
/// </summary>
public class UpdatePresetCommandHandler : IRequestHandler<UpdatePresetCommand, Result<Unit>>
{
    private readonly IPresetRepository _repository;

    private readonly IMapper _mapper;

    public UpdatePresetCommandHandler(IPresetRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Unit>> Handle(UpdatePresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId).ConfigureAwait(false);

        if (preset == null)
        {
            return Result<Unit>.Invalid("Preset with this Id wasn`t found");
        }

        var newPreset = _mapper.Map<Preset>(request);
        await _repository.Update(newPreset).ConfigureAwait(false);

        return Result<Unit>.Empty();
    }
}
