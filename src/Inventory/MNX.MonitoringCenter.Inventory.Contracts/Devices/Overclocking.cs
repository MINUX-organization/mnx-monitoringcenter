using MessagePack;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices;

/// <summary>
/// Разгон.
/// </summary>
[Union(0, typeof(CpuOverclocking))]
[Union(1, typeof(GpuOverclocking))]
[Union(2, typeof(NvidiaGpuOverclocking))]
[Union(3, typeof(AmdGpuOverclocking))]
[Union(4, typeof(IntelGpuOverclocking))]
public abstract record Overclocking { }
