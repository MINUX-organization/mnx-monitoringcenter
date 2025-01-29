using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases;

/// <summary>
/// Команда на установку окончания времени действия инвентаризации.
/// </summary>
/// <param name="RigId"></param>
public sealed record RigDisconnectedEvent(Guid RigId) : INotification;


/// <summary>
/// Обработчик команды <see cref="RigDisconnectedEvent"/>.
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
        return _repository.SetInventoryExpirationDate(notification.RigId, cancellationToken);
    }
}
