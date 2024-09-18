using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;

/// <summary>
/// Команда редактирования пресета
/// </summary>
public class EditPresetCommand : IValidatableCommand<PresetModel>
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
    public Guid UserId { get; }

    public EditPresetCommand(Guid id, PresetInputModel model, Guid userId)
    {
        Id = id;
        Model = model;
        UserId = userId;
    }
}
