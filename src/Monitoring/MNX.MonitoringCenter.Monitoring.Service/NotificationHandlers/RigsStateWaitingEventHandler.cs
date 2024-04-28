using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об ожидании состояния ригов.
/// </summary>
public class RigsStateWaitingEventHandler : INotificationHandler<RigsStateWaitingEvent>
{
    private readonly IPubSub _pubSub;

    public RigsStateWaitingEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(RigsStateWaitingEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new RigsStateWaitingMessage
        {
            UserId = notification.UserId,
            ConnectionId = notification.SubscriberId
        }, cancellationToken: cancellationToken);
    }
}
