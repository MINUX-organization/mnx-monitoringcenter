namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Пул
/// </summary>
public class Pool : IEquatable<Pool>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Домен
    /// </summary>
    public required string Domain {  get; set; }

    /// <summary>
    /// Порт
    /// </summary>
    public int Port { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Криптовалюта
    /// </summary>
    public Cryptocurrency? Cryptocurrency { get; set; }

    /// <summary>
    /// Идентификатор пользователя
    /// </summary>
    public long UserId { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Pool pool)
        {
            return Equals(pool);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Pool? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Domain == other.Domain && Port == other.Port;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Domain, Port);
    }
}