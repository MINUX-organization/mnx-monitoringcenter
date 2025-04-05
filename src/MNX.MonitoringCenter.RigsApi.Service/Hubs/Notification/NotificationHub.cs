using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;

namespace MNX.MonitoringCenter.RigsApi.Service.Hubs.Notification;

/// <summary>
/// Хаб нотификаций.
/// </summary>
[Authorize]
public class NotificationHub : Hub<INotificationHubClient> { }