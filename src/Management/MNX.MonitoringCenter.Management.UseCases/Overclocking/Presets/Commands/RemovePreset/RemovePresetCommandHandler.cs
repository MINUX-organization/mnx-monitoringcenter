using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.RemovePreset;

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
        await _repository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
