using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu;

/// <summary>
/// Технология параллельных вычислений.
/// </summary>
[ComplexType]
public class ParallelComputingTechnology
{
    /// <summary>
    /// Тип.
    /// </summary>
    public ParallelComputingTechnologyEnum Type { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public string Version { get; set; }
}
