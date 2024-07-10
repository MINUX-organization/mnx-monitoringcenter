using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об ожидании динамических данных ригов.
/// </summary>
public class DynamicDataWaitingEventHandler : INotificationHandler<DynamicDataWaitingEvent>
{
    private readonly IPubSub _pubSub;

    public DynamicDataWaitingEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(DynamicDataWaitingEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new StatisticsStreamStartCommand
        {
            UserId = notification.UserId,
            ObservableObjectsType = ObservableObjectsType.Rigs // todo:
        }, cancellationToken: cancellationToken);
    }
}
