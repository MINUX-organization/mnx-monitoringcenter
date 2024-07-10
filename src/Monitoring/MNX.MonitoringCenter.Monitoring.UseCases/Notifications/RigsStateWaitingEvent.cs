using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие об ожидании состояния ригов.
/// </summary>
public class RigsStateWaitingEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string SubscriberId { get; }

    public RigsStateWaitingEvent(long userId, string subscriberId)
    {
        UserId = userId;
        SubscriberId = subscriberId;
    }
}
