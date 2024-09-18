namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Конфиг для таргета полётного листа.
/// </summary>
public class FlightSheetTargetConfig : IEquatable<FlightSheetTargetConfig>
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; set; }

    /// <summary>
    /// Пул.
    /// </summary>
    public Pool? Pool { get; set; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    public string? PoolPassword { get; set; }

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
        if (obj is FlightSheetTargetConfig config)
        {
            return Equals(config);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(FlightSheetTargetConfig? other)
    {
        if (other is null)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return PoolId == other.PoolId &&
               PoolPassword == other.PoolPassword &&
               WalletId == other.WalletId;
    }

    /// <inheritdoc/>
    public override int GetHashCode()
    {
        return HashCode.Combine(Pool, Wallet, PoolPassword);
    }
}
