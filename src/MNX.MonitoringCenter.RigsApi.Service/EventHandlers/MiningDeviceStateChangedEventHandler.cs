using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.RigsApi.Service.Hubs.Notification;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers;

/// <summary>
/// Обработчик события <see cref="MiningDeviceStateChangedEvent"/>.
/// </summary>
public class MiningDeviceStateChangedEventHandler
    : INotificationHandler<MiningDeviceStateChangedEvent>
{
    private readonly IHubContext<NotificationHub, INotificationHubClient> _hubContext;

    public MiningDeviceStateChangedEventHandler(IHubContext<NotificationHub, INotificationHubClient> hubContext)
    {
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public Task Handle(MiningDeviceStateChangedEvent notification,
                       CancellationToken cancellationToken)
    {
        return _hubContext.Clients.User(notification.ClientId).MiningDeviceStateChanged();
    }
}