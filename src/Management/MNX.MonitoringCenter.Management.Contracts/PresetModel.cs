using MNX.MonitoringCenter.Management.Core;

namespace MNX.MonitoringCenter.Management.Contracts;

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
    public string Name { get; set; }    

    /// <summary>
    /// Название GPU
    /// </summary>
    public string GpuName { get; set; }

    /// <summary>
    /// Модель с разгоном
    /// </summary>
    public OverclockingModel Overclocking { get; set; } 
}
