namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;

/// <summary>
/// Получено сообщение об неудачной установке разгона для видеокарты.
/// </summary>
public class OverclockingSettingFailMessage
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Идентификатор соединения веб-клиента.
    /// </summary>
    public string ConnectionId { get; set; }

    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid CardId { get; set; }

    /// <summary>
    /// Сообщение об ошибке.
    /// </summary>
    public string Message { get; set; }
}
