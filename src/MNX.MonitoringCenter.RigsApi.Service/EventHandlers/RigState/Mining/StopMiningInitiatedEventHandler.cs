using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState.Mining;

/// <summary>
/// Обработчик <see cref="StopMiningInitiatedEvent"/>.
/// </summary>
public class StopMiningInitiatedEventHandler : INotificationHandler<StopMiningInitiatedEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    ///
    public StopMiningInitiatedEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    ///
    public Task Handle(StopMiningInitiatedEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new Management.Agent.Commands.Mining.StopMiningCommand(),
            new Guid[] { notification.RigId },
            notification.InitiatorId,
            cancellationToken: cancellationToken
        );
    }
}
