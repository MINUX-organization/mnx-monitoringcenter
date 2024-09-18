namespace MNX.MonitoringCenter.Management.Contracts.Presets;

/// <summary>
/// Модель для пресета
/// </summary>
public class PresetModel
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название пресета
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Название GPU
    /// </summary>
    public required string GpuName { get; set; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public required OverclockingModel Overclocking { get; set; }
}
