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
    private readonly IRigRepository _repository;

    public RigDisconnectedEventHandler(IRigRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task Handle(RigDisconnectedEvent notification, CancellationToken cancellationToken)
    {
        return _repository.SwitchToOffline(notification.RigId);
    }
}
