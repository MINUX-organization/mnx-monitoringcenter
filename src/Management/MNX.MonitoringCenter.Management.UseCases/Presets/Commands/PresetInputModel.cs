using MNX.MonitoringCenter.Management.UseCases.Commands.Presets;

namespace MNX.MonitoringCenter.Management.UseCases.Presets.Commands;

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
    /// Название GPU
    /// </summary>
    public string? GpuName { get; }

    /// <summary>
    /// Модель разгона
    /// </summary>
    public OverclockingInputModel Overclocking { get; }

    public PresetInputModel(string name, string? gpuName, OverclockingInputModel overclocking)
    {
        Name = name;
        GpuName = gpuName;
        Overclocking = overclocking;
    }
}
