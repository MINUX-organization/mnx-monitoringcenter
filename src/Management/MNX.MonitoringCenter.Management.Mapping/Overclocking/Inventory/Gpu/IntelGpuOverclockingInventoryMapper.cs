using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;

using IntelGpuOverclockingCore = Core.Overclocking.Gpu.IntelGpuOverclocking;
using IntelGpuOverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking.IntelGpuOverclocking;
using OverclockingInventory = MonitoringCenter.Inventory.Contracts.Devices.Overclocking;

public class IntelGpuOverclockingInventoryMapper : IOverclockingInventoryMapper<IntelGpuOverclockingInventory, IntelGpuOverclockingCore>
{
    /// <inheritdoc/>
    public IOverclocking MapToCoreEntity(IntelGpuOverclockingInventory model)
    {
        return new IntelGpuOverclockingCore() { };
    }

    /// <inheritdoc/>
    public OverclockingInventory MapToModel(IntelGpuOverclockingCore entity)
    {
        return new IntelGpuOverclockingInventory() { };
    }
}
