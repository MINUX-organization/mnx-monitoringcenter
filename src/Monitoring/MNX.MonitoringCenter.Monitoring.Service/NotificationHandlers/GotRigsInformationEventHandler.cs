using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.Service.Messages;
using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события о получении информации о ригах.
/// </summary>
public class GotRigsInformationEventHandler : INotificationHandler<GotRigsInformationEvent>
{
    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    public GotRigsInformationEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext ?? throw new ArgumentNullException(nameof(monitoringHubContext));
    }

    public async Task Handle(GotRigsInformationEvent notification, CancellationToken cancellationToken)
    {
        await _monitoringHubContext.Clients.Client(notification.SubscriberId)
            .ReceivedRigsInformation(notification.Information.Rigs);

        await _monitoringHubContext.Clients.Client(notification.SubscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalRigsCount,
            NewData = notification.Information.Rigs
        });

        await _monitoringHubContext.Clients.Client(notification.SubscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalGpusCount,
            NewData = notification.Information.Rigs
        });

        await _monitoringHubContext.Clients.Client(notification.SubscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalCpusCount,
            NewData = notification.Information.Rigs
        });
    }
}
