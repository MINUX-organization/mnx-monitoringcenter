using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts.Presets;
using MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Команда сохранение пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommand : IUserableValidatableCommand<PresetModel>
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
