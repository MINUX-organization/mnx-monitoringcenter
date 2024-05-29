using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

public class OverclockingSettingFailEventHandler : INotificationHandler<OverclockingSettingFailEvent>
{
    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    public OverclockingSettingFailEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext;
    }

    public async Task Handle(OverclockingSettingFailEvent notification, CancellationToken cancellationToken)
    {
        await _monitoringHubContext.Clients.Client(notification.ConnectionId)
            .ReceivedOverclockingSettingFailEvent(notification.CardId, notification.Message);
    }
}
