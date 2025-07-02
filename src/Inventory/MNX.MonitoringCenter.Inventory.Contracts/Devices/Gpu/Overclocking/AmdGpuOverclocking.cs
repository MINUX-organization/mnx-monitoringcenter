namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

/// <summary>
/// Разгон видеокарты модели Amd.
/// </summary>
public record AmdGpuOverclocking : GpuOverclocking
{
    /// <summary>
    /// Блокировка частоты ядра.
    /// </summary>
    public int? CoreClockLock { get; init; }

    /// <summary>
    /// Уровни частоты ядра.
    /// </summary>
    public int? CoreClockState { get; init; }

    /// <summary>
    /// Частота видеопамяти.
    /// </summary>
    public int? MemoryClockLock { get; init; }

    /// <summary>
    /// P-состояния памяти: частотные режимы в зависимости от нагрузки.
    /// </summary>
    public int? MemoryClockState { get; init; }

    /// <summary>
    /// Напряжение на ядро GPU.
    /// </summary>
    public int? CoreVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения ядра.
    /// </summary>
    public int? CoreVoltageOffset { get; init; }

    /// <summary>
    /// Напряжение видеопамяти.
    /// </summary>
    public int? MemoryVoltage { get; init; }

    /// <summary>
    /// Напряжение контроллера памяти.
    /// </summary>
    public int? MemoryControllerVoltage { get; init; }

    /// <summary>
    /// Настройки памяти.
    /// </summary>
    public string? MemoryTweak { get; init; }

    /// <summary>
    /// Автоматический разгон.
    /// </summary>
    public bool? EnhancedOverclock { get; init; }

    /// <summary>
    /// Альтернативное снижение напряжения.
    /// </summary>
    public bool? AlternativeDownVoltage { get; init; }

    /// <summary>
    /// Частота SoC (системной части GPU).
    /// </summary>
    public int? SocFrequency { get; init; }

    /// <summary>
    /// Напряжение SoC (системной части GPU).
    /// </summary>
    public int? SocVoltage { get; init; }
}
