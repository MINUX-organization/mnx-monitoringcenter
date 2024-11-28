namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

/// <summary>
/// Модель конфига для майнинга монеты.
/// </summary>
public class MiningCoinConfigModel
{
    /// <summary>
    /// Модель пула.
    /// </summary>
    public required PoolModel Pool { get; set; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    public string? PoolPassword { get; set; }

    /// <summary>
    /// Модель кошелька.
    /// </summary>
    public required WalletModel Wallet { get; set; }
}
