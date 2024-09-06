using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Температура видеокарты.
/// </summary>
[ComplexType]
public record GpuTemperature
{
    /// <summary>
    /// Ядро.
    /// </summary>
    public required RangeValue Core { get; init; }

    /// <summary>
    /// Память.
    /// </summary>
    public required RangeValue Memory { get; init; }
}
