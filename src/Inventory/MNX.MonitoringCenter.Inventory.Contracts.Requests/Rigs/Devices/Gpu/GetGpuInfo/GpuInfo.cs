using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpuInfo;

/// <summary>
/// Информация о видеокарте.
/// </summary>
public record GpuInfo : GpuInformation
{
    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid Id { get; init; }
}
