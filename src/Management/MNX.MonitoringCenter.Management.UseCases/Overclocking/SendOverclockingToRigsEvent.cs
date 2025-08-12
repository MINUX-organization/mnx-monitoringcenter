using MediatR;
using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking;
using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;
using MNX.RigCommander.MessageQueue.Clients.Bus;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

using OverclockingInventory = Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Команда отправки разгона майнинг-устройств на риги.
/// </summary>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="Devices"> Список майнинг-устройств. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record SendOverclockingToRigsEvent(IOverclocking Overclocking,
                                          List<MiningDeviceInfo> Devices,
                                          Guid UserId) : INotification;

/// <summary>
/// Обработчик команды <see cref="SendOverclockingToRigsEvent"/>.
/// </summary>
public class SendOverclockingToRigsEventHandler : INotificationHandler<SendOverclockingToRigsEvent>
{
    private readonly IOverclockingToFanOverclockingAgentMapper<IOverclocking, FanOverclocking> _overclockingToFanOverclockingAgentMapper;

    private readonly IOverclockingInventoryMapper<OverclockingInventory, IOverclocking> _inventoryOverclockingMapper;

    private readonly IQueueBusClient _queueClient;

    ///
    public SendOverclockingToRigsEventHandler(IOverclockingToFanOverclockingAgentMapper<IOverclocking, FanOverclocking> overclockingToFanOverclockingAgentMapper,
                                              IOverclockingInventoryMapper<OverclockingInventory, IOverclocking> inventoryOverclockingMapper,
                                              IQueueBusClient queueClient)
    {
        _overclockingToFanOverclockingAgentMapper = overclockingToFanOverclockingAgentMapper ??
            throw new ArgumentNullException(nameof(overclockingToFanOverclockingAgentMapper));
        _inventoryOverclockingMapper = inventoryOverclockingMapper ??
            throw new ArgumentNullException(nameof(inventoryOverclockingMapper));
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
    }

    ///
    public Task Handle(SendOverclockingToRigsEvent request, CancellationToken cancellationToken)
    {
        var rigOverclocking = _inventoryOverclockingMapper.MapToModel(request.Overclocking);
        var fanOverclocking = _overclockingToFanOverclockingAgentMapper.MapToFanOverclockingModel(request.Overclocking);

        var allDeviceIds = request.Devices.Select(x => x.Id).ToArray();

        var command = new SetOverclockingCommand(rigOverclocking, fanOverclocking, allDeviceIds);

        return _queueClient.Enqueue(command,
                                    request.Devices.Select(x => x.RigId!.Value).Distinct().ToArray(),
                                    request.UserId,
                                    cancellationToken: cancellationToken);
    }
}
