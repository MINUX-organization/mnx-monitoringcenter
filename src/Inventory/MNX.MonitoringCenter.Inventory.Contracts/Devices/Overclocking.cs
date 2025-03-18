using MessagePack;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices;

/// <summary>
/// Разгон.
/// </summary>
[Union(0, typeof(CpuOverclocking))]
[Union(1, typeof(GpuOverclocking))]
public abstract record Overclocking { }
