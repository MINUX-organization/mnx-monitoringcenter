namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

public class SavePresetInputModel
{
    /// <summary>
    /// Название пресета
    /// </summary>
    public string Name { get; }
    
    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; }

    /// <summary>
    /// Модель разгона
    /// </summary>
    public OverclockingInputModel Overclocking { get; }

    public SavePresetInputModel(string name, string gpuName, OverclockingInputModel overclocking)
    {
        Name = name;
        GpuName = gpuName;
        Overclocking = overclocking;
    }
}
