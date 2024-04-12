using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.Service.Messages;
using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateTotalDynamicData;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик об изменении обобщённых динамических данных.
/// </summary>
public class UpdateTotalDynamicDataEventHandler : INotificationHandler<UpdateTotalDynamicDataEvent>
{
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _hubContext;

    public UpdateTotalDynamicDataEventHandler(IHubContext<MonitoringHub, IMonitoringClient> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task Handle(UpdateTotalDynamicDataEvent notification, CancellationToken cancellationToken)
    {
        await _hubContext.Clients.User(notification.UserId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalPower,
                NewData = notification.Total.Power
            });

        await _hubContext.Clients.User(notification.UserId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalShares,
                NewData = notification.Total.Shares
            });

        await _hubContext.Clients.User(notification.UserId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalCoinsList,
                NewData = notification.Total.CoinsStatistics
            });
    }
}
