using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Software;

/// <summary>
/// Dto инвентаризации программного обеспечения.
/// </summary>
[Table("software_inventory")]
public class SoftwareInventoryDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public long Id { get; init; }

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
    /// Версия Агента.
    /// </summary>
    public required string AgentVersion { get; init; }

    /// <summary>
    /// Версия менеджера аппаратного обеспечения.
    /// </summary>
    public required string HardwareManagerVersion { get; init; }

    /// <summary>
    /// Майнеры.
    /// </summary>
    /// <remarks>
    /// Ключ - название майнера. Значение - версия майнера.
    /// </remarks>
    public Dictionary<string, string> Miners { get; init; } = new();
}
