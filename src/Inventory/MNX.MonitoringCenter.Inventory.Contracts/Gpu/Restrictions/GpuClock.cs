using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Разгон видеокарты.
/// </summary>
[ComplexType]
public record GpuClock
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
