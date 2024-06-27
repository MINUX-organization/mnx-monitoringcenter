using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об ожидании установки разгона видеокарты.
/// </summary>
public class OverclockingSettingWaitingEventHandler : INotificationHandler<OverclockingSettingWaitingEvent>
{
    private readonly IPubSub _pubSub;

    public OverclockingSettingWaitingEventHandler(IPubSub pubSub)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    public async Task Handle(OverclockingSettingWaitingEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new OverclockingSettingWaitingMessage()
        {
            UserId = notification.UserId,
            ConnectionId = notification.ConnectionId,
            CardId = notification.CardId,
            Overclocking = notification.Overclocking
        }, cancellationToken: cancellationToken);
    }
}
