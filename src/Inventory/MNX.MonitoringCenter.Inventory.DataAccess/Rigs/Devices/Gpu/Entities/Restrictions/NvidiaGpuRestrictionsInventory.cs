using Microsoft.EntityFrameworkCore;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;

/// <summary>
/// Ограничения разгона видеокарты модели Nvidia.
/// </summary>
[Owned]
public class NvidiaGpuRestrictionsInventory : GpuRestrictionsInventory
{
    /// <summary>
    /// Ограничения фиксации частоты процессора.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockCoreLock { get; init; }

    /// <summary>
    /// Ограничения смещения частоты процессора.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockCoreOffset { get; init; }

    /// <summary>
    /// Ограничения фиксации частоты видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockMemoryLock { get; init; }

    /// <summary>
    /// Ограничения смещения частоты видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory ClockMemoryOffset { get; init; }

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
    /// Ограничения смещения напряжения видеопамяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory VoltageMemoryOffset { get; init; }
}
