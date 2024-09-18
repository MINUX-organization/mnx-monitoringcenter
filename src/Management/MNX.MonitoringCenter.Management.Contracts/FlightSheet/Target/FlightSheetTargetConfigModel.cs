namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;

/// <summary>
/// Модель конфига таргета полётного листа.
/// </summary>
public class FlightSheetTargetConfigModel
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
