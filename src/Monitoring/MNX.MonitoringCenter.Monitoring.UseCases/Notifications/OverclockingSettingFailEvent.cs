using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ошибке применения разгона видеокарте.
/// </summary>
public class OverclockingSettingFailEvent : INotification
{
    /// <summary>
    /// Идентификатор соединения веб-клиента.
    /// </summary>
    public string ConnectionId { get; }

    /// <summary>
    /// Идентификатор видеокарты.
    /// </summary>
    public Guid CardId { get; }

    /// <summary>
    /// Сообщения об ошибке.
    /// </summary>
    public string[] Messages { get; }

    public OverclockingSettingFailEvent(string connectionId, Guid cardId, string[]? messages)
    {
        ConnectionId = connectionId;
        CardId = cardId;
        Messages = messages ?? Array.Empty<string>();
    }
}
