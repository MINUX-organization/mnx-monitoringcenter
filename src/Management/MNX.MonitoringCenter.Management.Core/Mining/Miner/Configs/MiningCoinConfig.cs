namespace MNX.MonitoringCenter.Management.Core.Mining.Miner.Configs;

/// <summary>
/// Конфиг для майнинга монеты.
/// </summary>
public class MiningCoinConfig : IEquatable<MiningCoinConfig>
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
    private string? _poolPassword;
    public string? PoolPassword
    {
        get => _poolPassword;
        set => _poolPassword = value?.Trim();
    }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; set; }

    /// <summary>
    /// Кошелек.
    /// </summary>
    public Wallet? Wallet { get; set; }

    /// <summary>
    /// Получить признак валидности конфига для майнинга монеты.
    /// </summary>
    /// <param name="errors"> Ошибки. </param>
    /// <returns> Признак валидности. </returns>
    public bool IsValid(out IReadOnlyCollection<string> errors)
    {
        bool isValid = true;
        var e = new List<string>(2);

        if (Wallet is null)
        {
            e.Add($"Wallet is required!");
            errors = e;
            return false;
        }

        if (Pool is null)
        {
            e.Add($"Pool is required!");
            errors = e;
            return false;
        }

        if (!Wallet.Cryptocurrency!.Equals(Pool.Cryptocurrency))
        {
            e.Add($"Pool ({PoolId}) do not correlates with wallet ({WalletId}) by cryptocurrency!");
            isValid = false;
        }

        if (PoolPassword != null && PoolPassword.Contains(' '))
        {
            e.Add($"Pool password {PoolPassword} cannot contain space characters");
            isValid = false;
        }

        errors = e;
        return isValid;
    }

    /// <inheritdoc/>
    public override bool Equals(object? obj)
    {
        if (obj is MiningCoinConfig config)
        {
            return Equals(config);
        }

        return false;
    }

    /// <inheritdoc/>
    public bool Equals(MiningCoinConfig? other)
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
