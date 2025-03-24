using MediatR;
using AutoMapper;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking;

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
    private readonly IMapper _mapper;

    private readonly IQueueBusClient _queueClient;

    public SendOverclockingToRigsEventHandler(IMapper mapper, IQueueBusClient queueClient)
    {
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
    }

    public Task Handle(SendOverclockingToRigsEvent request, CancellationToken cancellationToken)
    {
        var rigOverclocking = _mapper.Map<Inventory.Contracts.Devices.Overclocking>(request.Overclocking);

        var allDeviceIds = request.Devices.Select(x => x.Id).ToArray();

        var command = new Agent.Commands.Overclocking.SetOverclockingCommand(rigOverclocking, allDeviceIds);

        return _queueClient.Enqueue(command,
                                    request.Devices.Select(x => x.RigId!.Value).Distinct().ToArray(),
                                    request.UserId,
                                    cancellationToken: cancellationToken);
    }
}
