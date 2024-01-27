using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.Presets.RemovePreset;

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
        var preset = await _repository.GetById(request.Id);

        if (preset == null)
        {
            return Result<Unit>.Invalid("Preset with this id wasn`t found");
        }

        await _repository.Remove(preset);
        return Result<Unit>.Empty();
    }
}
