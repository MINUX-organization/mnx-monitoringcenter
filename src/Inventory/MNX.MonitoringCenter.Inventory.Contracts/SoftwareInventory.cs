namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Инвентаризация программного обеспечения.
/// </summary>
public record SoftwareInventory
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public required string MinuxVersion { get; init; }

    /// <summary>
    /// Версия Linux.
    /// </summary>
    public required string LinuxVersion { get; init; }

    /// <summary>
    /// Версия AMD драйвера.
    /// </summary>
    public required string AmdDriverVersion { get; init; }

    /// <summary>
    /// Версия драйвера Nvidia.
    /// </summary>
    public required string NvidiaDriverVersion { get; init; }

    /// <summary>
    /// Версия драйвера Intel.
    /// </summary>
    public required string IntelDriverVersion { get; init; }

    /// <summary>
    /// Версия OpenCL.
    /// </summary>
    public required string OpenCLVersion { get; init; }

    /// <summary>
    /// Версия CUDA.
    /// </summary>
    public required string CudaVersion { get; init; }

    /// <summary>
    /// Майнеры.
    /// </summary>
    /// <remarks>
    /// Ключ - название майнера. Значение - версия майнера.
    /// </remarks>
    public Dictionary<string, string> Miners { get; init; } = new();
}
