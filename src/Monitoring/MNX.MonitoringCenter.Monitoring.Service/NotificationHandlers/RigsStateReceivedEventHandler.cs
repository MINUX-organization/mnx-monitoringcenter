using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Abstractions;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события о получении состояния ригов.
/// </summary>
public class RigsStateReceivedEventHandler : INotificationHandler<RigsStateReceivedEvent>
{
    /// <summary>
    /// Посредник.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _monitoringHubContext;

    public RigsStateReceivedEventHandler(IHubContext<MonitoringHub, IMonitoringClient> monitoringHubContext)
    {
        _monitoringHubContext = monitoringHubContext ?? throw new ArgumentNullException(nameof(monitoringHubContext));
    }

    public async Task Handle(RigsStateReceivedEvent notification, CancellationToken cancellationToken)
    {
        var rigsIds = await _mediator.Send(new GetRigsIdsQuery(notification.Specification), cancellationToken);

        var response = (IEnumerable<RigState?>)MapToResponse(rigsIds.ToList(), notification.RigsState.ToArray());

        await _monitoringHubContext.Clients.Client(notification.SubscriberId).ReceivedRigsState(response);
    }

    /// <summary>
    /// Подготавливает данные к ответу.
    /// </summary>
    /// <param name="rigsIds"> Идентификаторы отфильтрованных ригов. </param>
    /// <param name="rigs"> Данные ригов. </param>
    /// <returns> Подготовленные к отправке клиенту данные ригов. </returns>
    private static List<IRig?> MapToResponse(List<Guid> rigsIds, IEnumerable<IRig> rigs) // todo: вынести
    {
        List<IRig?> response = new();

        for (int i = 0; i < rigsIds.Count; i++)
        {
            var rig = rigs.FirstOrDefault(x => x.Id == rigsIds[i]);

            if (rig is not null)
            {
                response.Add(rig);
                continue;
            }

            response.Add(null);
        }

        return response;
    }
}
