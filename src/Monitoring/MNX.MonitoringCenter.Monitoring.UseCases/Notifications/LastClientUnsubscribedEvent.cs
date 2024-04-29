using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об отписке последнего пользователя от наблюдателя.
/// </summary>
public class LastClientUnsubscribedEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    public LastClientUnsubscribedEvent(long userId)
    {
        UserId = userId;
    }
}
