using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Команда сохранение пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommand : IValidatableCommand<PresetModel>
{
    /// <summary>
    /// Входная модель SavePreset.
    /// </summary>
    public SavePresetInputModel SavePresetModel { get; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public SavePresetCommand(Guid userId, SavePresetInputModel model)
    {
        UserId = userId;
        SavePresetModel = model;
    }
}
