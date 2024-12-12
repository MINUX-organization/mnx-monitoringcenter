using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения разгона видеокарты.
/// </summary>
[ComplexType]
public record GpuClockRestrictions
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
