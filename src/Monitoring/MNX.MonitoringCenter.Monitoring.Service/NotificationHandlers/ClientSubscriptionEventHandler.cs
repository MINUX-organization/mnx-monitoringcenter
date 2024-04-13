using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события о подписке клиента.
/// </summary>
public class ClientSubscriptionEventHandler : INotificationHandler<ClientSubscriptionEvent>
{
    private readonly IPubSub _pubSub;

    public ClientSubscriptionEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(ClientSubscriptionEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new RigsStateWaitingMessage
        {
            UserId = notification.UserId,
            ConnectionId = notification.SubscriberId
        }, cancellationToken: cancellationToken);

        if (notification.SubscribersCount == 1)
        {
            await _pubSub.PublishAsync(new StatisticsStreamStartCommand
            {
                UserId = notification.UserId,
                ObservableObjectsType = ObservableObjectsType.Rigs
            }, cancellationToken: cancellationToken);
        }
    }
}
