using MNX.MonitoringCenter.Management.Contracts.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.EditPreset;

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
    public IOverclockingModel Overclocking { get; }

    public EditPresetModel(string name, IOverclockingModel overclocking)
    {
        Name = name;
        Overclocking = overclocking;
    }
}
