using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Команда сохранение пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommand : IValidatableCommand<PresetModel>
{
    /// <summary>
    /// Модель пресета.
    /// </summary>
    public PresetInputModel Model { get; }
    
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public SavePresetCommand(Guid userId, PresetInputModel model)
    {
        UserId = userId;
        Model = model;
    }
}
