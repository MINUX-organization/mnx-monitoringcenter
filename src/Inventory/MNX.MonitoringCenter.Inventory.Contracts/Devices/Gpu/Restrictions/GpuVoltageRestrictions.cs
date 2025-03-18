using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения напряжения видеокарты.
/// </summary>
[ComplexType]
public record GpuVoltageRestrictions
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
