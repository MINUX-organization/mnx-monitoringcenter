using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices;
using MNX.MonitoringCenter.Inventory.UseCases.Devices;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Реализация <see cref="IDevicesRepository"/>.
/// </summary>
public partial class InventoryRepository : IDevicesRepository
{
    public IAsyncEnumerable<Device> GetList(InventorySpecification specification, DeviceType? deviceTypes)
    {
        var inventoryQuery = GetInventoryBySpecification(specification);

        IQueryable<Device> devicesQuery = Enumerable.Empty<Device>().AsQueryable();

        // cpu
        if (deviceTypes is null || deviceTypes.Value.HasFlag(DeviceType.CPU))
        {
            var cpuQuery = inventoryQuery.SelectMany(inventory => inventory.Cpus.Select(cpu
                    => new Device(cpu.Id, inventory.RigId, DeviceType.CPU)));

            devicesQuery = devicesQuery.Union(cpuQuery);
        }

        // drives
        if (deviceTypes is null || deviceTypes.Value.HasFlag(DeviceType.Drive))
        {
            var drivesQuery = inventoryQuery.SelectMany(inventory => inventory.Drives.Select(drive
                    => new Device(drive.Id, inventory.RigId, DeviceType.Drive)));

            devicesQuery = devicesQuery.Union(drivesQuery);
        }

        // gpu
        if (deviceTypes is null || deviceTypes.Value.HasFlag(DeviceType.GPU))
        {
            var gpuQuery = inventoryQuery.SelectMany(inventory => inventory.Gpus.Select(gpu
                    => new Device(gpu.Id, inventory.RigId, DeviceType.GPU)));

            devicesQuery = devicesQuery.Union(gpuQuery);
        }

        // motherboard
        if (deviceTypes is null || deviceTypes.Value.HasFlag(DeviceType.Motherboard))
        {
            var gpuQuery = inventoryQuery.Select(inventory
                    => new Device(inventory.Motherboard.Id, inventory.RigId, DeviceType.GPU));

            devicesQuery = devicesQuery.Union(gpuQuery);
        }

        // network adapters
        if (deviceTypes is null || deviceTypes.Value.HasFlag(DeviceType.NetworkAdapter))
        {
            var adaptersQuery = inventoryQuery.SelectMany(inventory => inventory.NetworkAdapters.Select(adapter
                    => new Device(adapter.Id, inventory.RigId, DeviceType.NetworkAdapter)));

            devicesQuery = devicesQuery.Union(adaptersQuery);
        }

        return devicesQuery.AsAsyncEnumerable();
    }
}
