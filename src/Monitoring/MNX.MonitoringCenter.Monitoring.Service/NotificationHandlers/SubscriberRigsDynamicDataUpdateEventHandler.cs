using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Contracts.Abstractions;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateDynamicData;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об обновлении у подписчика динамических данных ригов.
/// </summary>
public class SubscriberRigsDynamicDataUpdateEventHandler
    : INotificationHandler<SubscriberRigsDynamicDataUpdateEvent>
{
    /// <summary>
    /// Посредник.
    /// </summary>
    private readonly IMediator _mediator;

    /// <summary>
    /// Контекст хаба мониторинга.
    /// </summary>
    private readonly IHubContext<MonitoringHub, IMonitoringClient> _hubContext;

    public SubscriberRigsDynamicDataUpdateEventHandler(IMediator mediator,
                                                       IHubContext<MonitoringHub, IMonitoringClient> hubContext)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _hubContext = hubContext ?? throw new ArgumentNullException(nameof(hubContext));
    }

    public async Task Handle(SubscriberRigsDynamicDataUpdateEvent notification, CancellationToken cancellationToken)
    {
        var rigsIds = await _mediator.Send(
            new GetRigsIdsQuery(new Specification(notification.Specification.UserId,
                                                  notification.Specification.SearchString,
                                                  notification.Specification.FilterString,
                                                  notification.Specification.FilterArguments)),
            cancellationToken);

        var response = (IEnumerable<RigDynamicData?>)MapToResponse(rigsIds.ToList(), notification.RigsDynamicData);

        await _hubContext.Clients.Client(notification.SubscriberId).ReceivedRigsDynamicData(response);

        if (notification.HashRateByObservableCoin is not null)
        {
            await _hubContext.Clients.Client(notification.SubscriberId)
                    .ReceivedCurrentHashRate(notification.HashRateByObservableCoin);
        }
    }

    /// <summary>
    /// Подготавливает данные к ответу.
    /// </summary>
    /// <param name="rigsIds"> Идентификаторы отфильтрованных ригов. </param>
    /// <param name="rigs"> Данные ригов. </param>
    /// <returns> Подготовленные к отправке клиенту данные ригов. </returns>
    private static List<IRig?> MapToResponse(List<Guid> rigsIds, IEnumerable<IRig> rigs)
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
