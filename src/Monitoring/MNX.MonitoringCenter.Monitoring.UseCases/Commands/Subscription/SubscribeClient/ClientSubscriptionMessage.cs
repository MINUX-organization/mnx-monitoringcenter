using MediatR;

namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient;

/// <summary>
/// Сообщение о подписки клиента.
/// </summary>
public class ClientSubscriptionMessage : IRequest<SubscribeClientResult>
{
    /// <summary>
    /// Кол-во подписчиков включая текущего.
    /// </summary>
    public long SubscribersCount { get; }

    /// <summary>
    /// Модель подписки.
    /// </summary>
    public SubscriptionModel SubscriptionModel { get; }

    public ClientSubscriptionMessage(long subscribersCount, SubscriptionModel subscriptionModel)
    {
        SubscribersCount = subscribersCount;
        SubscriptionModel = subscriptionModel;
    }
}
