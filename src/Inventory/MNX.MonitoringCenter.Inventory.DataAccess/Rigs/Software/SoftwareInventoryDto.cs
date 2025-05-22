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
    public string? MinuxVersion { get; init; }

    /// <summary>
    /// Версия Linux.
    /// </summary>
    public string? LinuxVersion { get; init; }

    /// <summary>
    /// Версия AMD драйвера для видеокарты.
    /// </summary>
    public string? AmdGpuDriverVersion { get; init; }

    /// <summary>
    /// Версия драйвера Nvidia для видеокарты.
    /// </summary>
    public string? NvidiaGpuDriverVersion { get; init; }

    /// <summary>
    /// Версия драйвера Intelдля видеокарты.
    /// </summary>
    public string? IntelGpuDriverVersion { get; init; }

    /// <summary>
    /// Версия OpenCL.
    /// </summary>
    public string? OpenCLVersion { get; init; }

    /// <summary>
    /// Версия CUDA.
    /// </summary>
    public string? CudaVersion { get; init; }

    /// <summary>
    /// Версия Агента.
    /// </summary>
    public required string AgentVersion { get; init; }

    /// <summary>
    /// Версия менеджера аппаратного обеспечения.
    /// </summary>
    public string? HardwareManagerVersion { get; init; }

    /// <summary>
    /// Майнеры.
    /// </summary>
    /// <remarks>
    /// Ключ - название майнера. Значение - массив версий майнера.
    /// </remarks>
    public Dictionary<string, string[]> Miners { get; init; } = [];
}
