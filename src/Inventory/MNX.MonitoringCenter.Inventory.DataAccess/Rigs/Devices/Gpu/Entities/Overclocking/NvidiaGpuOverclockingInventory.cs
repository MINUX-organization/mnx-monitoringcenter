using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;

/// <summary>
/// Параметры разгона видеокарты модели Nvidia.
/// </summary>
[Owned]
public class NvidiaGpuOverclockingInventory : GpuOverclockingInventory
{
    /// <summary>
    /// Фиксация частоты процессора.
    /// </summary>
    [Column("nvidia_core_clock_lock")]
    public int? CoreClockLock { get; init; }

    /// <summary>
    /// Смещение частоты процессора.
    /// </summary>
    [Column("nvidia_core_clock_offset")]
    public int? CoreClockOffset { get; init; }

    /// <summary>
    /// Фиксация частоты памяти.
    /// </summary>
    [Column("nvidia_memory_clock_lock")]
    public int? MemoryClockLock { get; init; }

    /// <summary>
    /// Смещение частоты памяти.
    /// </summary>
    [Column("nvidia_memory_clock_offset")]
    public int? MemoryClockOffset { get; init; }

    /// <summary>
    /// Напряжение процессора.
    /// </summary>
    [Column("nvidia_core_voltage")]
    public int? CoreVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения процессора.
    /// </summary>
    [Column("nvidia_core_voltage_offset")]
    public int? CoreVoltageOffset { get; init; }

    /// <summary>
    /// Напряжение памяти.
    /// </summary>
    [Column("nvidia_memory_voltage")]
    public int? MemoryVoltage { get; init; }

    /// <summary>
    /// Смещение напряжения памяти.
    /// </summary>
    [Column("nvidia_memory_voltage_offset")]
    public int? MemoryVoltageOffset { get; init; }
}
