using AutoMapper;
using EasyNetQ;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.NotificationHandlers;

/// <summary>
/// Обработчик события об установке разгона видеокарты.
/// </summary>
public class SetOverclockingEventHandler : INotificationHandler<SetOverclockingEvent>
{
    private readonly IPubSub _pubSub;

    private readonly IMapper _mapper;

    public SetOverclockingEventHandler(IPubSub pubSub, IMapper mapper)
    {
        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task Handle(SetOverclockingEvent notification, CancellationToken cancellationToken)
    {
        await _pubSub.PublishAsync(new SetOverclockingMessage()
        {
            UserId = notification.UserId,
            ConnectionId = notification.ConnectionId,
            CardId = notification.CardId,
            Overclocking = _mapper.Map<OverclockingModel>(notification.Overclocking)
        }, cancellationToken: cancellationToken);
    }
}
