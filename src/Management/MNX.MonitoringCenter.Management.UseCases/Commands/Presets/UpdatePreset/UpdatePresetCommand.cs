using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.UpdatePreset;

/// <summary>
/// Команда редактирования пресета
/// </summary>
public class UpdatePresetCommand : IValidatableCommand<PresetModel>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; }

    /// <summary>
    /// Модель пресета
    /// </summary>
    public SavePresetInputModel SavePresetModel { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public UpdatePresetCommand(Guid id, SavePresetInputModel model, long userId)
    {
        Id = id;
        SavePresetModel = model;
        UserId = userId;
    }
}
