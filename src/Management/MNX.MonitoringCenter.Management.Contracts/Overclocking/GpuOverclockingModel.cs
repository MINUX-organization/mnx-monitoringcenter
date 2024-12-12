using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для видеокарты.
/// </summary>
public record GpuOverclockingModel : GpuOverclocking, IOverclockingModel { }
