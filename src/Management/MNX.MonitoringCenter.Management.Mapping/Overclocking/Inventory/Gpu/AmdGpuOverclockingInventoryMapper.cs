using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;

using AmdGpuOverclockingCore = Core.Overclocking.Gpu.AmdGpuOverclocking;
using AmdGpuOverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking;
using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Реализация <see cref="IOverclockingInventoryMapper{TModel, TEntity}"/>
/// для сущностей <see cref="AmdGpuOverclockingInventory"/> и <see cref="AmdGpuOverclockingCore"/>.
/// </summary>
public class AmdGpuOverclockingInventoryMapper :
    IOverclockingInventoryMapper<AmdGpuOverclockingInventory, AmdGpuOverclockingCore>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(AmdGpuOverclockingInventory model)
    {
        var fanOverclocking = new FanOverclockingWithTargetSpeed()
        {
            TargetSpeed = model.FanSpeed ?? 0
        };

        return new AmdGpuOverclockingCore()
        {
            FanOverclocking = fanOverclocking,
            PowerLimit = model.PowerLimit ?? 0,
            CoreClockLock = model.CoreClockLock ?? 0,
            CoreClockState = model.CoreClockState ?? 0,
            CoreVoltage = model.CoreVoltage ?? 0,
            CoreVoltageOffset = model.CoreVoltageOffset ?? 0,
            MemoryClockLock = model.MemoryClockLock ?? 0,
            MemoryClockState = model.MemoryClockState ?? 0,
            MemoryControllerVoltage = model.MemoryControllerVoltage ?? 0,
            MemoryVoltage = model.MemoryVoltage ?? 0,
            SocFrequency = model.SocFrequency ?? 0,
            SocVoltage = model.SocVoltage ?? 0,
            AlternativeDownVoltage = model.AlternativeDownVoltage ?? false,
            EnhancedOverclock = model.EnhancedOverclock ?? false,
            MemoryTweak = model.MemoryTweak
        };
    }

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(AmdGpuOverclockingCore entity)
    {
        return new AmdGpuOverclockingInventory()
        {
            PowerLimit = entity.PowerLimit,
            CoreClockLock = entity.CoreClockLock,
            CoreClockState = entity.CoreClockState,
            CoreVoltage = entity.CoreVoltage,
            CoreVoltageOffset = entity.CoreVoltageOffset,
            MemoryClockLock = entity.MemoryClockLock,
            MemoryClockState = entity.MemoryClockState,
            MemoryControllerVoltage = entity.MemoryControllerVoltage,
            MemoryVoltage = entity.MemoryVoltage,
            SocFrequency = entity.SocFrequency,
            SocVoltage = entity.SocVoltage,
            AlternativeDownVoltage = entity.AlternativeDownVoltage,
            EnhancedOverclock = entity.EnhancedOverclock,
            MemoryTweak = entity.MemoryTweak
        };
    }
}
