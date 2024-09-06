using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Напряжение видеокарты.
/// </summary>
[ComplexType]
public record GpuVoltage
{
    /// <summary>
    /// Ядро.
    /// </summary>
    public required GpuChangingValue Core { get; init; }

    /// <summary>
    /// Память.
    /// </summary>
    public required GpuChangingValue Memory { get; init; }
}
