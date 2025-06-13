using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState.Power;

/// <summary>
/// Обработчик <see cref="RigRebootInitiatedEvent"/>.
/// </summary>
public class RigRebootInitiatedEventHandler
    : INotificationHandler<RigRebootInitiatedEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    ///
    public RigRebootInitiatedEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    ///
    public Task Handle(RigRebootInitiatedEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new Management.Agent.Commands.RebootCommand(),
            new Guid[] { notification.RigId },
            notification.InitiatorId,
            cancellationToken: cancellationToken
        );
    }
}