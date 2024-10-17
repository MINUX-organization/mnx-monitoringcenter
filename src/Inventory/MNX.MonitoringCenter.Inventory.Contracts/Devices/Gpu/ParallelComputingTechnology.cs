using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

/// <summary>
/// Технология параллельных вычислений.
/// </summary>
[ComplexType]
public record ParallelComputingTechnology
{
    /// <summary>
    /// Тип.
    /// </summary>
    public ParallelComputingTechnologyEnum Type { get; init; }

    /// <summary>
    /// Версия.
    /// </summary>
    public required string Version { get; init; }
}
