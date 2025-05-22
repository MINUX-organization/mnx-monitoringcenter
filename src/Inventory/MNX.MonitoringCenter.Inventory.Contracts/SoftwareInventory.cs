namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Инвентаризация программного обеспечения.
/// </summary>
public class SoftwareInventory : IEquatable<SoftwareInventory>
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
    /// <remarks>
    /// Ключ - название майнера. Значение - версия майнера.
    /// </remarks>
    public Dictionary<string, string[]> Miners { get; init; } = [];

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is SoftwareInventory software)
        {
            return Equals(software);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(SoftwareInventory? other)
    {
        if (other == null) return false;

        if (ReferenceEquals(this, other)) return true;

        return MinuxVersion == other.MinuxVersion &&
               LinuxVersion == other.LinuxVersion &&
               AmdGpuDriverVersion == other.AmdGpuDriverVersion &&
               NvidiaGpuDriverVersion == other.NvidiaGpuDriverVersion &&
               IntelGpuDriverVersion == other.IntelGpuDriverVersion &&
               OpenCLVersion == other.OpenCLVersion &&
               CudaVersion == other.CudaVersion &&
               Miners.SequenceEqual(other.Miners);
    }

    public override int GetHashCode()
    {
        var minersHash = 0;

        foreach (var miner in Miners)
        {
            minersHash = HashCode.Combine(miner.Key, miner.Value);
        }

        return HashCode.Combine(MinuxVersion, LinuxVersion, AmdGpuDriverVersion, NvidiaGpuDriverVersion,
                                IntelGpuDriverVersion, OpenCLVersion, CudaVersion) + minersHash;
    }
}
