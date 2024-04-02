using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Команда редактирования пресета
/// </summary>
public class UpdatePresetCommand : IValidatableCommand<Unit>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пресета
    /// </summary>
    public PresetInputModel Model { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public UpdatePresetCommand(Guid id, PresetInputModel model, long userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
