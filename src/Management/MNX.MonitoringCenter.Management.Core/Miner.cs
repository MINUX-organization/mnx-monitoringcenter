namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Майнер.
/// </summary>
public sealed class Miner : IEquatable<Miner>
{
    public required string Name { get; set; }

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

        return Name == other.Name;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Name);
    }
}