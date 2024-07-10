using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об ожидании подписчиком истории скорости хеширования.
/// </summary>
public class HashRateHistoryWaitingEventHandler : INotificationHandler<SubscriberAwaitHashRateHistory>
{
    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    public HashRateHistoryWaitingEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext ?? throw new ArgumentNullException(nameof(monitoringHubContext));
    }

    public async Task Handle(SubscriberAwaitHashRateHistory notification, CancellationToken cancellationToken)
    {
        await _monitoringHubContext.Clients.Client(notification.SubscriberId)
                .ReceivedHashRateForAPeriod(notification.HashRateHistory);
    }
}
