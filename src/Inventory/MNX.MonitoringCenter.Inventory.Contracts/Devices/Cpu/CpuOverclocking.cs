using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

/// <summary>
/// Разгон процессора.
/// </summary>
[ComplexType]
public record CpuOverclocking : Overclocking
{
    /// <summary>
    /// Фиксированная частота ядра.
    /// </summary>
    public int CoreClockLock { get; init; }

    /// <summary>
    /// Напряжение.
    /// </summary>
    public int CoreVoltage { get; init; }
}
