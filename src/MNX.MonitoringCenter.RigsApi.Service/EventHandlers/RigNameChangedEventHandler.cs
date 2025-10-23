using MediatR;
using EasyNetQ;

using MNX.MonitoringCenter.RigsApi.Contracts.Events;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers;

/// <summary>
/// Обработчик события <see cref="RigNameChangedEvent"/>.
/// </summary>
public class RigNameChangedEventHandler : INotificationHandler<RigNameChangedEvent>
{
    private readonly IPubSub _bus;
    
    public RigNameChangedEventHandler(IPubSub bus)
    {
        ArgumentNullException.ThrowIfNull(bus, nameof(bus));
        _bus = bus;
    }
    
    /// <inheritdoc/>
    public Task Handle(RigNameChangedEvent notification, CancellationToken cancellationToken)
    {
        RigNameChangedIntegrationEvent message = new(notification.RigId, notification.NewName);
        return _bus.PublishAsync(message, cancellationToken);
    }
}