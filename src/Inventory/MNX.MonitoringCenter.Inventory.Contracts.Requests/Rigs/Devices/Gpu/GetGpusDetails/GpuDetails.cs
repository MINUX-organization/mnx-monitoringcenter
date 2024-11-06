namespace MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;

/// <summary>
/// Детали видеокарты.
/// </summary>
public record GpuDetails : Contracts.Devices.Gpu.Gpu
{
    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; init; }

    /// <summary>
    /// Версия драйвера для работы с видеокартой.
    /// </summary>
    public string? DriverVersion { get; init; }
}
