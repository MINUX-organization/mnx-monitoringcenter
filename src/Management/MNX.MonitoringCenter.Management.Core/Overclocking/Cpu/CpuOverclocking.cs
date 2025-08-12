using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;

/// <summary>
/// Разгон процессора.
/// </summary>
public class CpuOverclocking : IOverclocking
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => OverclockingTargetDeviceType.CPU;
    }

    /// <summary>
    /// Фиксированная частота ядра.
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Напряжение.
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new CpuOverclocking()
        {
            Id = Id,
            CoreClockLock = CoreClockLock,
            CoreVoltage = CoreVoltage
        };
    }
}
