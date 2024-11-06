using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetRigDevices;

namespace MNX.MonitoringCenter.Management.Controllers;

/// <summary>
/// Потребитель сообщений от ригов.
/// </summary>
public class RigConsumer : IConsumeAsync<RigInventoryMsg>
{
    private readonly IMediator _mediator;

    public RigConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public Task ConsumeAsync(RigInventoryMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(
            new SetRigDevicesCommand(message.RigId, message.RigOwnerId,
                                     message.Inventory.Gpus, message.Inventory.Cpus),
            cancellationToken);
    }
}
