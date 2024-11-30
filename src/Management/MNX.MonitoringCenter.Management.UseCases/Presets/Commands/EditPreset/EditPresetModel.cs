using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Commands.EditPreset;

/// <summary>
/// Модель для редактирования пресета.
/// </summary>
public class EditPresetModel
{
    /// <summary>
    /// Название пресета.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Модель разгона.
    /// </summary>
    public OverclockingInputModel Overclocking { get; }

    public EditPresetModel(string name, OverclockingInputModel overclocking)
    {
        Name = name;
        Overclocking = overclocking;
    }
}
