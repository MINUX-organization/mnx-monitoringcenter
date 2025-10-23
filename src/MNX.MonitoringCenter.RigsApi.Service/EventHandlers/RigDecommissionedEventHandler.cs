using EasyNetQ;
using MediatR;

using MNX.MonitoringCenter.RigsApi.Contracts.Events;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers;

/// <summary>
/// Обработчик события <inheritdoc cref="RigDecommissionedEvent"/>.
/// </summary>
public class RigDecommissionedEventHandler : INotificationHandler<RigDecommissionedEvent>
{
    private readonly IPubSub _bus;

    ///
    public RigDecommissionedEventHandler(IPubSub bus)
    {
        ArgumentNullException.ThrowIfNull(bus, nameof(bus));
        _bus = bus;
    }
    
    /// <inheritdoc/>
    public Task Handle(RigDecommissionedEvent notification, CancellationToken cancellationToken)
    {
        RigDecommissionedIntegrationEvent message = new(notification.RigId);
        return _bus.PublishAsync(message, cancellationToken);
    }
}