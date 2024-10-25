namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusInfo;

/// <summary>
/// Детали видеокарты.
/// </summary>
public record GpuDetails : Contracts.Devices.Gpu.Gpu
{
    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; set; }

    /// <summary>
    /// Версия драйвера для работы с видеокартой.
    /// </summary>
    public string? DriverVersion { get; set; }
}
