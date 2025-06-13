using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents.Power;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.RigsApi.Service.EventHandlers.RigState.Power;

/// <summary>
/// Обработчик <see cref="RigPowerOffInitiatedEvent"/>.
/// </summary>
public class RigPowerOffInitiatedEventHandler
    : INotificationHandler<RigPowerOffInitiatedEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    ///
    public RigPowerOffInitiatedEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    ///
    public Task Handle(RigPowerOffInitiatedEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new Management.Agent.Commands.PowerOffCommand(),
            new Guid[] { notification.RigId },
            notification.InitiatorId,
            cancellationToken: cancellationToken
        );
    }
}
