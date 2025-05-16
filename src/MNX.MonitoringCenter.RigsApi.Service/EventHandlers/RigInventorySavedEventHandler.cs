using MediatR;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers;

/// <summary>
/// Обработчик события <see cref="RigInventorySavedEvent"/>.
/// </summary>
public class RigInventorySavedEventHandler : INotificationHandler<RigInventorySavedEvent>
{
    private readonly IMediator _mediator;

    public RigInventorySavedEventHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task Handle(RigInventorySavedEvent notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;
        var inventory = message.Inventory;

        return _mediator.Send(new SetRigDevicesCommand(message.RigId,
                                                       message.RigOwnerId,
                                                       inventory.Gpus,
                                                       inventory.Cpus), cancellationToken);
    }
}
