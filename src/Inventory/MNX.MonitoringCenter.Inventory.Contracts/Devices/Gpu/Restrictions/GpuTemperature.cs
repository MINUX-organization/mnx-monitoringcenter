using System.ComponentModel.DataAnnotations.Schema;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

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
