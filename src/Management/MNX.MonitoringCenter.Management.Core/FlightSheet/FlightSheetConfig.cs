namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Конфиг для полётного листа.
/// </summary>
public class FlightSheetConfig : IEquatable<FlightSheetConfig>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; set; }

    /// <summary>
    /// Пул.
    /// </summary>
    public Pool? Pool { get; set; }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; set; }

    /// <summary>
    /// Кошелек.
    /// </summary>
    public Wallet? Wallet { get; set; }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is FlightSheetConfig config)
        {
            return Equals(config);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FlightSheetConfig? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return PoolId == other.PoolId && WalletId == other.WalletId;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Pool, Wallet);
    }
}
