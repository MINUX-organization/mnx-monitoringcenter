using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;

/// <summary>
/// Команда удаления пресета
/// </summary>
public class RemovePresetCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// УНикальный идентификатор пресета
    /// </summary>
    public Guid Id { get; }

    public RemovePresetCommand(Guid id)
    {
        Id = id;
    }
}
