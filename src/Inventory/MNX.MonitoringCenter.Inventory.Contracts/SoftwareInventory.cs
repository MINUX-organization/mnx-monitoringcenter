namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Инвентаризация программного обеспечения.
/// </summary>
public class SoftwareInventory
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public string MinuxVersion { get; set; }

    /// <summary>
    /// Версия Linux.
    /// </summary>
    public string LinuxVersion { get; set; }

    /// <summary>
    /// Версия AMD драйвера.
    /// </summary>
    public string AmdDriverVersion { get; set; }

    /// <summary>
    /// Версия драйвера Nvidia.
    /// </summary>
    public string NvidiaDriverVersion { get; set; }

    /// <summary>
    /// Версия драйвера Intel.
    /// </summary>
    public string IntelDriverVersion { get; set; }

    /// <summary>
    /// Версия OpenCL.
    /// </summary>
    public string OpenCLVersion { get; set; }

    /// <summary>
    /// Версия CUDA.
    /// </summary>
    public string CudaVersion { get; set; }
}
