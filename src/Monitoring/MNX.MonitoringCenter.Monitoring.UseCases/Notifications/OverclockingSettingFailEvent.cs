using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ошибке применения разгона видеокарте.
/// </summary>
public class OverclockingSettingFailEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; set; }

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

    public OverclockingSettingFailEvent(long userId, string connectionid, Guid cardId, string message)
    {
        UserId = userId;
        ConnectionId = connectionid;
        CardId = cardId;
        Message = message;
    }
}
