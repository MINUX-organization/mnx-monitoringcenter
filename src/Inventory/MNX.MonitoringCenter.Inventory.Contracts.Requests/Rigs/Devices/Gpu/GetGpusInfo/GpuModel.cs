namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusInfo;

/// <summary>
/// Модель видеокарты.
/// </summary>
public record GpuModel : Contracts.Devices.Gpu.Gpu
{
    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; init; }
}
