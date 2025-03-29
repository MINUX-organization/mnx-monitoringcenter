namespace MNX.MonitoringCenter.Management.Core.Overclocking;

/// <summary>
/// Модель с разгоном.
/// </summary>
public class GpuOverclocking : IOverclocking, IEquatable<GpuOverclocking>, ICloneable
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => OverclockingTargetDeviceType.GPU;
    }

    /// <summary>
    /// Фиксированная частота ядра
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Смещение частоты ядра
    /// </summary>
    public int CoreClockOffset { get; set; }

    /// <summary>
    /// Фиксированная частота памяти
    /// </summary>
    public int MemoryClockLock { get; set; }

    /// <summary>
    /// Смещение частоты памяти
    /// </summary>
    public int MemoryClockOffset { get; set; }

    /// <summary>
    /// Напряжение на ядре
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения на ядре
    /// </summary>
    public int CoreVoltageOffset { get; set; }

    /// <summary>
    /// Напряжение памяти
    /// </summary>
    public int MemoryVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения памяти
    /// </summary>
    public int MemoryVoltageOffset { get; set; }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Скорость вентилятора
    /// </summary>
    public int FanSpeed { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new GpuOverclocking()
        {
            Id = this.Id,
            CoreClockLock = this.CoreClockLock,
            CoreClockOffset = this.CoreClockOffset,
            MemoryClockLock = this.MemoryClockLock,
            MemoryClockOffset = this.MemoryClockOffset,
            CoreVoltage = this.CoreVoltage,
            CoreVoltageOffset = this.CoreVoltageOffset,
            MemoryVoltage = this.MemoryVoltage,
            MemoryVoltageOffset = this.MemoryVoltageOffset,
            PowerLimit = this.PowerLimit,
            FanSpeed = this.FanSpeed
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is GpuOverclocking overclocking)
        {
            return Equals(overclocking);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(GpuOverclocking? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return CoreClockLock == other.CoreClockLock &&
               CoreClockOffset == other.CoreClockOffset &&
               MemoryClockLock == other.MemoryClockLock &&
               MemoryClockOffset == other.MemoryClockOffset &&
               CoreVoltage == other.CoreVoltage &&
               CoreVoltageOffset == other.CoreVoltageOffset &&
               MemoryVoltage == other.MemoryVoltage &&
               MemoryVoltageOffset == other.MemoryVoltageOffset &&
               PowerLimit == other.PowerLimit &&
               FanSpeed == other.FanSpeed;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(CoreClockLock);
        hash.Add(CoreClockOffset);
        hash.Add(MemoryClockLock);
        hash.Add(MemoryClockOffset);
        hash.Add(CoreVoltage);
        hash.Add(CoreVoltageOffset);
        hash.Add(MemoryVoltage);
        hash.Add(MemoryVoltageOffset);
        hash.Add(PowerLimit);
        hash.Add(FanSpeed);

        return hash.ToHashCode();
    }
}
