using MediatR;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Events;

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
    private readonly IMiningDeviceRepository _repository;

    public RigDisconnectedEventHandler(IMiningDeviceRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task Handle(RigDisconnectedEvent notification, CancellationToken cancellationToken)
    {
        return _repository.SetStatusForRigDevices(notification.RigId, MiningDeviceLifeCycleStatus.Offline);
    }
}
