namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.MiningConfig;

/// <summary>
/// Входная модель с конфигом для майнинга монеты.
/// </summary>
public class MiningCoinConfigInputModel
{
    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; init; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    private string? _poolPassword;
    public string? PoolPassword
    {
        get => _poolPassword;
        init => _poolPassword = value?.Trim();
    }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; init; }
}
