namespace MNX.MonitoringCenter.Management.Core.Overclocking;

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
            Id = this.Id,
            CoreClockLock = this.CoreClockLock,
            CoreVoltage = this.CoreVoltage
        };
    }
}
