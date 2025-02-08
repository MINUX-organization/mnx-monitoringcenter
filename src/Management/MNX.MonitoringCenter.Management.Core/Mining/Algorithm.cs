namespace MNX.MonitoringCenter.Management.Core.Mining;

/// <summary>
/// Алгоритм.
/// </summary>
public class Algorithm : IEquatable<Algorithm>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Пользовательский идентификатор.
    /// </summary>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Признак доменного алгоритма.
    /// </summary>
    public bool IsDomain => UserId is null ? true : false;

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
