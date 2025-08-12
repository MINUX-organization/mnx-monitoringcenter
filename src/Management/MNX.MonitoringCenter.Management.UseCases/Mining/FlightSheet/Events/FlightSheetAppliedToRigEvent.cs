using MediatR;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Events;

/// <summary>
/// Событие о применении полётного листа на риг.
/// </summary>
/// <param name="RigId"> Идентификатор рига. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="DeviceFlightSheets"> Полётные листы майнинг устройств. </param>
public record FlightSheetAppliedToRigEvent(
    Guid RigId,
    Guid UserId,
    List<DeviceFLightSheet> DeviceFlightSheets) : INotification;


/// <summary>
/// Обработчик <see cref="FlightSheetAppliedToRigEvent"/>.
/// </summary>
public class FlightSheetAppliedToRigEventHandler : INotificationHandler<FlightSheetAppliedToRigEvent>
{
    private readonly IAgentCommandsMapper _agentCommandsMapper;

    private readonly IQueueBusClient _queueClient;

    ///
    public FlightSheetAppliedToRigEventHandler(IAgentCommandsMapper agentCommandsMapper, IQueueBusClient queueClient)
    {
        _agentCommandsMapper = agentCommandsMapper ?? throw new ArgumentNullException(nameof(agentCommandsMapper));
        _queueClient = queueClient ?? throw new ArgumentNullException(nameof(queueClient));
    }

    ///
    public Task Handle(FlightSheetAppliedToRigEvent notification, CancellationToken cancellationToken)
    {
        var message = new ApplyWorkerSettingsCommand(
            _agentCommandsMapper.MapToWorkerSettings(notification.DeviceFlightSheets));

        return _queueClient.Enqueue(
            message, new Guid[] { notification.RigId }, notification.UserId, null, null, cancellationToken);
    }
}
