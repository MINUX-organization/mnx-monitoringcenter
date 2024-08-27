using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Температура видеокарты.
/// </summary>
[ComplexType]
public sealed record GpuTemperature
{
    /// <summary>
    /// Ядро.
    /// </summary>
    public RangeValue Core { get; set; }

    /// <summary>
    /// Память.
    /// </summary>
    public RangeValue Memory { get; set; }
}
