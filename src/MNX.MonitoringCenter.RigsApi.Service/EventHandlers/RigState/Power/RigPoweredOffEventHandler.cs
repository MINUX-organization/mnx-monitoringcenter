using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState.Power;

/// <summary>
/// Обработчик <see cref="RigPoweredOffEvent"/>.
/// </summary>
public class RigPoweredOffEventHandler : INotificationHandler<RigPoweredOffEvent>
{
    private readonly IMediator _mediator;

    ///
    public RigPoweredOffEventHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    ///
    public Task Handle(RigPoweredOffEvent notification, CancellationToken cancellationToken)
    {
        return Task.WhenAll(
            _mediator.Publish(new Inventory.UseCases.RigDisconnectedEvent(notification.RigId), cancellationToken),
            _mediator.Publish(new Management.UseCases.RigDisconnectedEvent(notification.RigId), cancellationToken)
        );
    }
}
