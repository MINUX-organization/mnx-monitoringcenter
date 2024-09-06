using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;

/// <summary>
/// Обработчик команды удаления пресета
/// </summary>
public class RemovePresetCommandHandler : IRequestHandler<RemovePresetCommand, Result<Unit>>
{
    private readonly IPresetRepository _repository;

    public RemovePresetCommandHandler(IPresetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(RemovePresetCommand request, CancellationToken cancellationToken)
    {
        var preset = await _repository.GetAvailableById(request.Id, request.UserId);

        if (preset != null)
        {
            await _repository.Remove(preset);
        }

        return Result<Unit>.Empty();
    }
}
