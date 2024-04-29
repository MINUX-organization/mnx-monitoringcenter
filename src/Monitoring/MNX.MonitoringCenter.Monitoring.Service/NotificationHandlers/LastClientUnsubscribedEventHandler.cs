using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об отписке последнего пользователя от наблюдателя.
/// </summary>
public class LastClientUnsubscribedEventHandler : INotificationHandler<LastClientUnsubscribedEvent>
{
    private readonly IPubSub _pubSub;

    public LastClientUnsubscribedEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(LastClientUnsubscribedEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new StatisticsStreamStopCommand
        {
            UserId = notification.UserId,
            ObservableObjectsType = ObservableObjectsType.Rigs // todo:
        }, cancellationToken: default);
    }
}
