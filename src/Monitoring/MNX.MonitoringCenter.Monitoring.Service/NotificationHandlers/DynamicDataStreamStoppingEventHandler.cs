using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об остановки потока динамических данных.
/// </summary>
public class DynamicDataStreamStoppingEventHandler : INotificationHandler<DynamicDataStreamStoppingEvent>
{
    private readonly IPubSub _pubSub;

    public DynamicDataStreamStoppingEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(DynamicDataStreamStoppingEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new StatisticsStreamStopCommand
        {
            UserId = notification.UserId,
            ObservableObjectsType = ObservableObjectsType.Rigs // todo:
        }, cancellationToken: default);
    }
}
