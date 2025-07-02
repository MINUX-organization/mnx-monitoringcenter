using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;

/// <summary>
/// Параметры разгона видеокарты модели Amd.
/// </summary>
[Owned]
public class AmdGpuOverclockingInventory : GpuOverclockingInventory
{
    /// <summary>
    /// Блокировка частоты процессора.
    /// </summary>
    [Column("amd_core_clock_lock")]
    public int? CoreClockLock { get; init; }

    /// <summary>
    /// Уровни частоты процессора.
    /// </summary>
    [Column("amd_core_clock_state")]
    public int? CoreClockState { get; init; }

    /// <summary>
    /// Напряжение процессора.
    /// </summary>
    [Column("amd_core_voltage")]
    public int? CoreVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения процессора.
    /// </summary>
    [Column("amd_core_voltage_offset")]
    public int? CoreVoltageOffset { get; init; }

    /// <summary>
    /// Частота видеопамяти.
    /// </summary>
    [Column("amd_memory_clock_lock")]
    public int? MemoryClockLock { get; init; }

    /// <summary>
    /// P-состояния памяти: частотные режимы в зависимости от нагрузки.
    /// </summary>
    [Column("amd_memory_clock_state")]
    public int? MemoryClockState { get; init; }

    /// <summary>
    /// Напряжение видеопамяти.
    /// </summary>
    [Column("amd_memory_voltage")]
    public int? MemoryVoltage { get; init; }

    /// <summary>
    /// Напряжение контроллера памяти.
    /// </summary>
    [Column("amd_memory_controller_voltage")]
    public int? MemoryControllerVoltage { get; init; }

    /// <summary>
    /// Настройки памяти.
    /// </summary>
    [Column("amd_memory_tweak")]
    public string? MemoryTweak { get; init; }

    /// <summary>
    /// Автоматический разгон.
    /// </summary>
    [Column("amd_enhanced_overclock")]
    public bool? EnhancedOverclock { get; init; }

    /// <summary>
    /// Альтернативное снижение напряжения.
    /// </summary>
    [Column("amd_alternative_down_voltage")]
    public bool? AlternativeDownVoltage { get; init; }

    /// <summary>
    /// Частота SoC (системной части GPU).
    /// </summary>
    [Column("amd_soc_frequency")]
    public int? SocFrequency { get; init; }

    /// <summary>
    /// Напряжение SoC.
    /// </summary>
    [Column("amd_soc_voltage")]
    public int? SocVoltage { get; init; }
}
