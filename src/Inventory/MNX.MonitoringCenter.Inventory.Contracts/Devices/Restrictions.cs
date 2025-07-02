using MessagePack;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices;

/// <summary>
/// Ограничения параметров устройств.
/// </summary>
[Union(0, typeof(CpuRestrictions))]
[Union(1, typeof(GpuRestrictions))]
public abstract record Restrictions { };
