using Microsoft.EntityFrameworkCore;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;

/// <summary>
/// Ограничения разгона видеокарты модели Amd.
/// </summary>
[Owned]
public class AmdGpuRestrictionsInventory : GpuRestrictionsInventory
{
    /// <summary>
    /// Ограничения фиксации частоты ядра. 
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockCoreLock { get; init; }

    /// <summary>
    /// Ограничения уровней частоты ядра.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockCoreState { get; init; }

    /// <summary>
    /// Ограничения частоты видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockMemoryLock { get; init; }

    /// <summary>
    /// Ограничения P-states частоты видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockMemoryState { get; init; }

    /// <summary>
    /// Ограничения фиксации напряжения процессора.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory VoltageCoreLock { get; init; }

    /// <summary>
    /// Ограничения смещения напряжения процессора.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory VoltageCoreOffset { get; init; }

    /// <summary>
    /// Ограничения фиксации напряжения видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory VoltageMemoryLock { get; init; }

    /// <summary>
    /// Ограничения напряжения контроллера памяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory VoltageMemoryController { get; init; }

    /// <summary>
    /// Ограничения частоты Soc (системной части GPU).
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory SocFrequency { get; init; }

    /// <summary>
    /// Ограничения напряжения Soc (системной части GPU).
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory SocVoltage { get; init; }
}
