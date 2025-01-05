using MediatR;
using MNX.MonitoringCenter.Management.Agent.Commands;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases;

/// <summary>
/// Событие об установки соединения рига с сервером.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
public sealed record RigConnectedEvent(Guid RigId) : INotification;


/// <summary>
/// Обработчик <see cref="RigConnectedEvent"/>.
/// </summary>
public class RigConnectedEventHandler : INotificationHandler<RigConnectedEvent>
{
    private readonly IQueueBusClient _messageQueueClient;

    public RigConnectedEventHandler(IQueueBusClient messageQueueClient)
    {
        _messageQueueClient = messageQueueClient
            ?? throw new ArgumentNullException(nameof(messageQueueClient));
    }

    public Task Handle(RigConnectedEvent notification, CancellationToken cancellationToken)
    {
        return _messageQueueClient.Enqueue(
            new CreateInventoryCommand(),
            new Guid[] { notification.RigId },
            Guid.Empty,
            cancellationToken: cancellationToken);
    }
}
