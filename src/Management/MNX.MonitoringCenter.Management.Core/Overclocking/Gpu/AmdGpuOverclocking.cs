
namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;

/// <summary>
/// Модель разгона видеокарты модели Amd.
/// </summary>
public class AmdGpuOverclocking : IOverclocking, IEquatable<AmdGpuOverclocking>
{
    /// <inheritdoc/>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <inheritdoc/>
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => OverclockingTargetDeviceType.AmdGPU;
    }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Скорость вентилятора
    /// </summary>
    public int FanSpeed { get; set; }

    /// <summary>
    /// Блокировка частоты ядра.
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Уровни частоты ядра.
    /// </summary>
    public int CoreClockState { get; set; }

    /// <summary>
    /// Напряжение на ядро GPU.
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения ядра.
    /// </summary>
    public int CoreVoltageOffset { get; set; }

    /// <summary>
    /// Частота видеопамяти.
    /// </summary>
    public int MemoryClockLock { get; set; }

    /// <summary>
    /// P-состояния памяти: частотные режимы в зависимости от нагрузки.
    /// </summary>
    public int MemoryClockState { get; set; }

    /// <summary>
    /// Напряжение видеопамяти.
    /// </summary>
    public int MemoryVoltage { get; set; }

    /// <summary>
    /// Напряжение контроллера памяти.
    /// </summary>
    public int MemoryControllerVoltage { get; set; }

    /// <summary>
    /// Настройки памяти.
    /// </summary>
    public string? MemoryTweak { get; set; }

    /// <summary>
    /// Автоматический разгон.
    /// </summary>
    public bool EnhancedOverclock { get; set; }

    /// <summary>
    /// Альтернативное снижение напряжения.
    /// </summary>
    public bool AlternativeDownVoltage { get; set; }

    /// <summary>
    /// Частота SoC (системной части GPU).
    /// </summary>
    public int SocFrequency { get; set; }

    /// <summary>
    /// Напряжение SoC.
    /// </summary>
    public int SocVoltage { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new AmdGpuOverclocking()
        {
            Id = Id,
            CoreClockLock = CoreClockLock,
            CoreClockState = CoreClockState,
            CoreVoltage = CoreVoltage,
            CoreVoltageOffset = CoreVoltageOffset,
            MemoryClockLock = MemoryClockLock,
            MemoryClockState = MemoryClockState,
            MemoryVoltage = MemoryVoltage,
            MemoryControllerVoltage = MemoryControllerVoltage,
            MemoryTweak = MemoryTweak,
            EnhancedOverclock = EnhancedOverclock,
            AlternativeDownVoltage = AlternativeDownVoltage,
            SocFrequency = SocFrequency,
            SocVoltage = SocVoltage,
            PowerLimit = PowerLimit,
            FanSpeed = FanSpeed
        };
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is AmdGpuOverclocking overclocking)
        {
            return Equals(overclocking);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(AmdGpuOverclocking? other)
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
               CoreClockState == other.CoreClockState &&
               CoreVoltage == other.CoreVoltage &&
               CoreVoltageOffset == other.CoreVoltageOffset &&
               MemoryClockLock == other.MemoryClockLock &&
               MemoryClockState == other.MemoryClockState &&
               MemoryVoltage == other.MemoryVoltage &&
               MemoryControllerVoltage == other.MemoryControllerVoltage &&
               MemoryTweak == other.MemoryTweak &&
               EnhancedOverclock == other.EnhancedOverclock &&
               AlternativeDownVoltage == other.AlternativeDownVoltage &&
               SocFrequency == other.SocFrequency &&
               SocVoltage == other.SocVoltage &&
               PowerLimit == other.PowerLimit &&
               FanSpeed == other.FanSpeed;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        HashCode hash = new();

        hash.Add(CoreClockLock);
        hash.Add(CoreClockState);
        hash.Add(CoreVoltage);
        hash.Add(CoreVoltageOffset);
        hash.Add(MemoryClockLock);
        hash.Add(MemoryClockState);
        hash.Add(MemoryVoltage);
        hash.Add(MemoryControllerVoltage);
        hash.Add(MemoryTweak);
        hash.Add(EnhancedOverclock);
        hash.Add(AlternativeDownVoltage);
        hash.Add(SocFrequency);
        hash.Add(SocVoltage);
        hash.Add(PowerLimit);
        hash.Add(FanSpeed);

        return hash.ToHashCode();
    }
}
