namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

/// <summary>
/// Входная модель с конфигом для таргета полётного листа.
/// </summary>
public class FlightSheetTargetConfigInputModel
{
    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; init; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    public string? PoolPassword { get; init; }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; init; }
}
