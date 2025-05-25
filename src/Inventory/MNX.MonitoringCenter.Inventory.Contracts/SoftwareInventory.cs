namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Инвентаризация программного обеспечения.
/// </summary>
public class SoftwareInventory
{
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
    /// Версия драйвера Intel для видеокарты.
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
    public List<MinerInventory> Miners { get; init; } = [];
}
