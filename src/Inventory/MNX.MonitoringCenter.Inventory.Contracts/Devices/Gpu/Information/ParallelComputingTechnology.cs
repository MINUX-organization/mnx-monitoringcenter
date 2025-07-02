namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Information;

/// <summary>
/// Технология параллельных вычислений.
/// </summary>
public record ParallelComputingTechnology
{
    /// <summary>
    /// Тип.
    /// </summary>
    public ParallelComputingTechnologyEnum Type { get; init; }

    /// <summary>
    /// Версия.
    /// </summary>
    public string? Version { get; init; }
}
