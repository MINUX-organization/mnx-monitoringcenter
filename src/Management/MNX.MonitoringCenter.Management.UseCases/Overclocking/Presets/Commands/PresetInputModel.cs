using MNX.MonitoringCenter.Management.Contracts.Overclocking;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands;

/// <summary>
/// Входная модель пресета.
/// </summary>
public class PresetInputModel
{
    /// <summary>
    /// Название пресета
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Название майнинг устройства.
    /// </summary>
    public string? DeviceName { get; }

    /// <summary>
    /// Модель разгона
    /// </summary>
    public IOverclockingModel Overclocking { get; }

    public PresetInputModel(string name, string? deviceName, IOverclockingModel overclocking)
    {
        Name = name;
        DeviceName = deviceName;
        Overclocking = overclocking;
    }
}
