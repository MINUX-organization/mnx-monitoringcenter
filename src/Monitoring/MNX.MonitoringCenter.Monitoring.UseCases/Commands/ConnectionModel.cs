namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands;

/// <summary>
/// Модель подключения.
/// </summary>
public class ConnectionModel
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

    /// <summary>
    /// Идентификатор подключения.
    /// </summary>
    public string ConnectionId { get; set; } = string.Empty;
}
