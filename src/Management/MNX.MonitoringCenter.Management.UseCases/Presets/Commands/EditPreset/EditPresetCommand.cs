using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands.EditPreset;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.EditPreset;

/// <summary>
/// Команда редактирования пресета
/// </summary>
public class EditPresetCommand : IUserableValidatableCommand<PresetModel>
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

    public EditPresetCommand(Guid id, EditPresetModel model, Guid userId)
    {
        Id = id;
        Model = new PresetInputModel(model.Name, null, model.Overclocking);
        UserId = userId;
    }
}
