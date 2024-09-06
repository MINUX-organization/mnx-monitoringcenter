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
    public Guid UserId { get; }

    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string SubscriberId { get; }

    public RigsStateWaitingEvent(Guid userId, string subscriberId)
    {
        UserId = userId;
        SubscriberId = subscriberId;
    }
}
