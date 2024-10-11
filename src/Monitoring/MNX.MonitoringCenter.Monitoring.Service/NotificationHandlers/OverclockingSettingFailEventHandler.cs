using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик уведомления об ошибке применения разгона видеокарте.
/// </summary>
public class OverclockingSettingFailEventHandler : INotificationHandler<OverclockingSettingFailEvent>
{
    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    //private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    /*public OverclockingSettingFailEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext;
    }*/

    /// <summary>
    /// Отправить уведомление клиенту.
    /// </summary>
    /// <param name="notification"> Уведомление. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public async Task Handle(OverclockingSettingFailEvent notification, CancellationToken cancellationToken)
    {
        //await _monitoringHubContext.Clients.Client(notification.ConnectionId)
        //    .ReceivedOverclockingSettingError(notification.CardId, notification.Messages);
    }
}
