using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

/// <summary>
/// Событие о подписки клиента.
/// </summary>
public class ClientSubscriptionEvent : INotification
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public long UserId { get; }

    /// <summary>
    /// Идентификатор подписчика.
    /// </summary>
    public string SubscriberId { get; }

    /// <summary>
    /// Кол-во подписчиков, включая текущего.
    /// </summary>
    public long SubscribersCount { get; }

    public ClientSubscriptionEvent(long userId, string subscriberId, long subscribersCount)
    {
        UserId = userId;
        SubscriberId = subscriberId;
        SubscribersCount = subscribersCount;
    }
}
