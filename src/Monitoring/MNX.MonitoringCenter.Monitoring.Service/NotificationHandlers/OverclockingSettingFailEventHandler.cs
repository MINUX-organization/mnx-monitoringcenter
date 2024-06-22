using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об ошибке применения разгона видеокарте.
/// </summary>
public class OverclockingSettingFailEventHandler : IRequestHandler<OverclockingSettingFailEvent, Result<Unit>>
{
    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    public OverclockingSettingFailEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext;
    }

    public async Task<Result<Unit>> Handle(OverclockingSettingFailEvent request, CancellationToken cancellationToken)
    {
        await _monitoringHubContext.Clients.Client(request.ConnectionId)
            .ReceivedOverclockingSettingFailEvent(request.CardId, request.Message);
        return Result<Unit>.Success(Unit.Value);
    }
}
