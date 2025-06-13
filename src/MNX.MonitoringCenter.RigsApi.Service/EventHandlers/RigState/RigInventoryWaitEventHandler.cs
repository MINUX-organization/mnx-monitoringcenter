using MediatR;
using MNX.MonitoringCenter.Management.Agent.Commands;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState;

/// <summary>
/// Обработчик <see cref="RigInventoryWaitEvent"/>.
/// </summary>
public class RigInventoryWaitEventHandler : INotificationHandler<RigInventoryWaitEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    ///
    public RigInventoryWaitEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    ///
    public Task Handle(RigInventoryWaitEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new CreateInventoryCommand(),
            new Guid[] { notification.RigId },
            Guid.Empty,
            cancellationToken: cancellationToken);
    }
}
