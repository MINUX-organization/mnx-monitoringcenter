using Kernel.UseCases;
using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Команда редактировния пресета
/// </summary>
public class UpdatePresetCommand : IValidateableCommand<Unit>
{
    /// <summary>
    /// Уникальный идентиификатор
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
