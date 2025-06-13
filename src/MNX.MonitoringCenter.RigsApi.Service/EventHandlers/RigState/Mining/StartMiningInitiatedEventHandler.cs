using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Mining;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState.Mining;

/// <summary>
/// Обработчик <see cref="StartMiningInitiatedEvent"/>.
/// </summary>
public class StartMiningInitiatedEventHandler : INotificationHandler<StartMiningInitiatedEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    ///
    public StartMiningInitiatedEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    ///
    public Task Handle(StartMiningInitiatedEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new Management.Agent.Commands.Mining.StartMiningCommand(),
            new Guid[] { notification.RigId },
            notification.InitiatorId,
            cancellationToken: cancellationToken
        );
    }
}
