namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Инвентаризация программного обеспечения.
/// </summary>
public class SoftwareInventory : IEquatable<SoftwareInventory>
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

        return Id.Equals(other.Id) &&
               MinuxVersion.Equals(other.MinuxVersion) &&
               LinuxVersion.Equals(other.LinuxVersion) &&
               AmdDriverVersion.Equals(other.AmdDriverVersion) &&
               NvidiaDriverVersion.Equals(other.NvidiaDriverVersion) &&
               IntelDriverVersion.Equals(other.IntelDriverVersion) &&
               OpenCLVersion.Equals(other.OpenCLVersion) &&
               CudaVersion.Equals(other.CudaVersion) &&
               Miners.SequenceEqual(other.Miners);
    }

    public override int GetHashCode()
    {
        var minersHash = 0;

        foreach (var miner in Miners)
        {
            minersHash = HashCode.Combine(miner.Key, miner.Value);
        }

        return HashCode.Combine(Id, MinuxVersion, LinuxVersion, AmdDriverVersion, NvidiaDriverVersion,
                                IntelDriverVersion, OpenCLVersion, CudaVersion) + minersHash;
    }
}
