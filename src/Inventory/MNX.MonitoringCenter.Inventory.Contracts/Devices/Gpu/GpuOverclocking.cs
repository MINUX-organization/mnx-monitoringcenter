using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

/// <summary>
/// Разгон видеокарты.
/// </summary>
[ComplexType]
public record GpuOverclocking : Overclocking
{
    /// <summary>
    /// Мощность.
    /// </summary>
    public int? PowerLimit { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int? FanSpeed { get; init; }

    /// <summary>
    /// Фиксированная частота ядра.
    /// </summary>
    public int? CoreClockLock { get; init; }

    /// <summary>
    /// Смещение частоты ядра.
    /// </summary>
    public int? CoreClockOffset { get; init; }

    /// <summary>
    /// Фиксированная частота памяти.
    /// </summary>
    public int? MemoryClockLock { get; init; }

    /// <summary>
    /// Смещение частоты памяти.
    /// </summary>
    public int? MemoryClockOffset { get; init; }

    /// <summary>
    /// Напряжение на ядре.
    /// </summary>
    public int? CoreVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения на ядре.
    /// </summary>
    public int CoreVoltageOffset { get; init; }

    /// <summary>
    /// Напряжение памяти.
    /// </summary>
    public int? MemoryVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения памяти.
    /// </summary>
    public int? MemoryVoltageOffset { get; init; }
}
