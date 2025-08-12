using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;

using NvidiaGpuOverclockingCore = Core.Overclocking.Gpu.NvidiaGpuOverclocking;
using NvidiaGpuOverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking;
using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Реализация <see cref="IOverclockingInventoryMapper"/> для сущностей
/// <see cref="NvidiaGpuOverclockingInventory"/> и <see cref="NvidiaGpuOverclockingCore"/>.
/// </summary>
public class NvidiaGpuOverclockingInventoryMapper : IOverclockingInventoryMapper<NvidiaGpuOverclockingInventory, NvidiaGpuOverclockingCore>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(NvidiaGpuOverclockingInventory model)
    {
        var fanOverclocking = new FanOverclockingWithTargetSpeed() { TargetSpeed = model.FanSpeed ?? 0 };
        return new NvidiaGpuOverclockingCore()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit ?? 0,
            CoreClockLock = model.CoreClockLock ?? 0,
            CoreClockOffset = model.CoreClockOffset ?? 0,
            CoreVoltage = model.CoreVoltage ?? 0,
            CoreVoltageOffset = model.CoreVoltageOffset ?? 0,
            MemoryClockLock = model.MemoryClockLock ?? 0,
            MemoryClockOffset = model.MemoryClockOffset ?? 0,
            MemoryVoltage = model.MemoryVoltage ?? 0,
            MemoryVoltageOffset = model.MemoryVoltageOffset ?? 0,
        };
    }

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(NvidiaGpuOverclockingCore entity)
    {
        return new NvidiaGpuOverclockingInventory()
        {
            PowerLimit = entity.PowerLimit,
            CoreClockLock = entity.CoreClockLock,
            CoreClockOffset = entity.CoreClockOffset,
            CoreVoltage = entity.CoreVoltage,
            CoreVoltageOffset = entity.CoreVoltageOffset,
            MemoryClockLock = entity.MemoryClockLock,
            MemoryClockOffset = entity.MemoryClockOffset,
            MemoryVoltage = entity.MemoryVoltage,
            MemoryVoltageOffset = entity.MemoryVoltageOffset,
        };
    }
}
