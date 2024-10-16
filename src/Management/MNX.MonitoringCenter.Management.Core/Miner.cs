using MNX.MonitoringCenter.Management.Core.Enums;

namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Майнер.
/// </summary>
public class Miner : IEquatable<Miner>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public required string Version { get; set; }

    /// <summary>
    /// Типы поддерживаемых девайсов.
    /// </summary>
    public DeviceEnum SupportedDevices { get; set; }

    /// <summary>
    /// Режим майнинга монет (для GPU).
    /// </summary>
    public GpuMiningModeEnum MiningMode { get; set; } = GpuMiningModeEnum.Single;

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Miner miner)
        {
            return Equals(miner);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Miner? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Name == other.Name && Version == other.Version;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Version);
    }
}