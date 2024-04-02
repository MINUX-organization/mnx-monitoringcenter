namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

public class SavePresetInputModel
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; }

    /// <summary>
    /// Модель пресета
    /// </summary>
    public PresetInputModel PresetModel { get; }

    public SavePresetInputModel(string gpuName, PresetInputModel model)
    {
        GpuName = gpuName;
        PresetModel = model;
    }
}
