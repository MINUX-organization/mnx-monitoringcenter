using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Cpu;

using CpuOverclockingCore = CpuOverclocking;
using CpuOverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Cpu.CpuOverclocking;
using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Реализация <see cref="IOverclockingInventoryMapper{TModel, TEntity}"/> для сущностей
/// <see cref="CpuOverclockingInventory"/> и <see cref="CpuOverclockingCore"/>.
/// </summary>
public class CpuOverclockingInventoryMapper : IOverclockingInventoryMapper<CpuOverclockingInventory, CpuOverclockingCore>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(CpuOverclockingInventory model)
    {
        return new CpuOverclockingCore()
        {
            CoreClockLock = model.CoreClockLock ?? 0,
            CoreVoltage = model.CoreVoltage ?? 0,
        };
    }

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(CpuOverclockingCore entity)
    {
        return new CpuOverclockingInventory()
        {
            CoreClockLock = entity.CoreClockLock,
            CoreVoltage = entity.CoreVoltage,
        };
    }
}
