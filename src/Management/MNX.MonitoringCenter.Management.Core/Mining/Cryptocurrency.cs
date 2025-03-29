namespace MNX.MonitoringCenter.Management.Core.Mining;

/// <summary>
/// Криптовалюта
/// </summary>
public class Cryptocurrency : IEquatable<Cryptocurrency>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Короткое название.
    /// </summary>
    public required string ShortName { get; set; }

    /// <summary>
    /// Полное название.
    /// </summary>
    public required string FullName { get; set; }

    /// <summary>
    /// Идентификатор алгоритма.
    /// </summary>
    public Guid AlgorithmId { get; set; }

    /// <summary>
    /// Алгоритм.
    /// </summary>
    public Algorithm? Algorithm { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    /// <remarks>
    /// Если значение идентификатора пользователя null,
    /// значит криптовалюта является доменной.
    /// </remarks>
    public Guid? UserId { get; init; }

    /// <summary>
    /// Получить булевый признак того,
    /// является ли криптовалюта доменной.
    /// </summary>
    /// <returns>
    /// <see langword="true"/>, если криптовалюта
    /// является доменной, иначе - <see langword="false"/>.
    /// </returns>
    public bool IsDomain()
    {
        return UserId is null;
    }

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
               AlgorithmId == other.AlgorithmId;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(ShortName, FullName, AlgorithmId);
    }
}