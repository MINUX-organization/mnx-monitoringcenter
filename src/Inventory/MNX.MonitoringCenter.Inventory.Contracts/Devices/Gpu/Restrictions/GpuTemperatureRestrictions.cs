using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения температуры видеокарты.
/// </summary>
[ComplexType]
public record GpuTemperatureRestrictions
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
