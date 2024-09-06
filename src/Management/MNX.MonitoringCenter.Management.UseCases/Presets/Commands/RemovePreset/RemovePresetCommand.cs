using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.RemovePreset;

/// <summary>
/// Команда удаления пресета
/// </summary>
public class RemovePresetCommand : IRequest<Result<Unit>>
{
    /// <summary>
    /// Уникальный идентификатор пресета
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public RemovePresetCommand(Guid id, Guid userId)
    {
        Id = id;
        UserId = userId;
    }
}
