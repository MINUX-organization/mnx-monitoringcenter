using MediatR;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;
using MNX.MonitoringCenter.RigsApi.UseCases.RigState;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers;

using RigId = Core.ValueObjects.RigId;

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

        var setRigDevicesCommand = new SetRigDevicesCommand(message.RigId,
                                                            message.RigOwnerId,
                                                            inventory.Gpus,
                                                            inventory.Cpus);

        var setInventoryCommand = new SetInventoryCommand(new RigId(message.RigId), notification.RigInventoryId);

        return Task.WhenAll(
            _mediator.Send(setRigDevicesCommand, cancellationToken),
            _mediator.Send(setInventoryCommand, cancellationToken)
        );
    }
}
