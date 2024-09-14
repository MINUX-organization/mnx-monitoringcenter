namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet;

/// <summary>
/// Модель конфига для полётного листа.
/// </summary>
public class FlightSheetConfigModel
{
    /// <summary>
    /// Модель пула.
    /// </summary>
    public PoolModel Pool { get; set; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    public string? PoolPassword { get; set; }

    /// <summary>
    /// Модель кошелька.
    /// </summary>
    public WalletModel Wallet { get; set; }
}
