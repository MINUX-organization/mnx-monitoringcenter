namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

/// <summary>
/// Входная модель с конфигом для таргета полётного листа.
/// </summary>
public class FlightSheetTargetConfigInputModel
{
    /// <summary>
    /// Поле пароля подключения к пулу.
    /// </summary>
    private string? _poolPassword;

    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; init; }

    /// <summary>
    /// Пароль подключения к пулу.
    /// </summary>
    public string? PoolPassword 
    { 
        get => _poolPassword; 
        init => _poolPassword = value;
    }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; init; }

    /// <summary>
    /// Убрать пробелы в начале и в конце <see cref="PoolPassword"/>.
    /// </summary>
    public void TrimGapsInPoolPassword()
    {
        _poolPassword = _poolPassword?.Trim();
    }
}
