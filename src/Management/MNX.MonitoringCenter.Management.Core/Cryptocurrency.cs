namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Криптовалюта
/// </summary>
public sealed class Cryptocurrency : IEquatable<Cryptocurrency>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Короткое название.
    /// </summary>
    public required string ShortName { get; set; }

    /// <summary>
    /// Полное название.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Алгоритм.
    /// </summary>
    public required string Algorithm { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Cryptocurrency cryptocurrency)
        {
            return Equals(cryptocurrency);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Cryptocurrency? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return ShortName == other.ShortName &&
               FullName == other.FullName &&
               Algorithm == other.Algorithm;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(ShortName, FullName, Algorithm);
    }
}