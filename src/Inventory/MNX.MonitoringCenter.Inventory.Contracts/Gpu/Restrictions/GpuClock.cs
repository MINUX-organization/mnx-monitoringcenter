using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Разгон видеокарты.
/// </summary>
[ComplexType]
public sealed record GpuClock
{
    /// <summary>
    /// Ядро.
    /// </summary>
    public GpuChangingValue Core { get; set; }

    /// <summary>
    /// Память.
    /// </summary>
    public GpuChangingValue Memory { get; set; }
}
