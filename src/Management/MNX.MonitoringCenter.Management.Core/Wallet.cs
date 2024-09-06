namespace MNX.MonitoringCenter.Management.Core;

/// <summary>
/// Кошелёк
/// </summary>
public sealed class Wallet : IEquatable<Wallet>
{
    /// <summary>
    /// Уникальный идентификатор
    /// </summary>
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    /// Имя
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Адрес
    /// </summary>
    public required string Address { get; set; }

    /// <summary>
    /// Идентификатор криптовалюты
    /// </summary>
    public Guid CryptocurrencyId { get; set; }

    /// <summary>
    /// Криптовалюта
    /// </summary>
    public Cryptocurrency? Cryptocurrency { get; set; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is Wallet wallet)
        {
            return Equals(wallet);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(Wallet? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return Address == other.Address;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Address);
    }
}