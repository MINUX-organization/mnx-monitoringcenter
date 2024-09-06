namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Алгоритм.
/// </summary>
public sealed class Algorithm : IEquatable<Algorithm>
{
    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Algorithm algorithm)
        {
            return Equals(algorithm);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Algorithm? other)
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
