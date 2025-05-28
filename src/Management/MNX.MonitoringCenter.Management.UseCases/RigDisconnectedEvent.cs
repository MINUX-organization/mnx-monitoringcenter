using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Событие об отключении рига от сервера.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public record RigDisconnectedEvent(Guid RigId) : INotification;


/// <summary>
/// Обработчик <see cref="RigDisconnectedEvent"/>.
/// </summary>
public class RigDisconnectedEventHandler : INotificationHandler<RigDisconnectedEvent>
{
    private IMediator _mediator;

    private readonly IRigRepository _repository;

    public RigDisconnectedEventHandler(IMediator mediator, IRigRepository repository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task Handle(RigDisconnectedEvent notification, CancellationToken cancellationToken)
    {
        await _repository.SwitchToOffline(notification.RigId);
        await _mediator.Publish(new MiningDeviceStateChangedEvent(notification.RigId.ToString()), cancellationToken);
    }
}
