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
    /// <remarks>
    /// Если имеет значение NULL, значит алгоритм является доменным.
    /// </remarks>
    public Guid? OwnerId { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Признак принадлежности алгоритма
    /// к доменным алгоритмам.
    /// </summary>
    /// <returns>
    /// <see langword="true"/>,
    /// если алгоритм доменный,
    /// иначе - <see langword="false"/>.
    /// </returns>
    public bool IsDomain()
    {
        return OwnerId is null;
    }

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
