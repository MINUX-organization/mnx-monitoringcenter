using Kernel.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Presets.SavePreset;

/// <summary>
/// Команда сохранение пресета для выбранной серии GPU
/// </summary>
public class SavePresetCommand : IValidateableCommand<Guid>
{
    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; }

    /// <summary>
    /// Модель пресета
    /// </summary>
    public PresetModel Model { get; }

    public SavePresetCommand(string gpuName, PresetModel model)
    {
        GpuName = gpuName;
        Model = model;
    }
}
