using MediatR;
using AutoMapper;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.Requests;
using MNX.RigCommander.MessageQueue.Clients.Bus;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Mining.MiningDevice;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.SetOverclocking;

/// <summary>
/// Команда отправки разгона майнинг-устройств на риги.
/// </summary>
/// <param name="Overclocking"> Разгон. </param>
/// <param name="Devices"> Список майнинг-устройств. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public record SendOverclockingToRigsCommand(IOverclocking Overclocking,
                                            List<MiningDeviceInfo> Devices,
                                            Guid UserId)
    : IUserableRequest<Result<Unit>>;

/// <summary>
/// Обработчик команлы <see cref="SendOverclockingToRigsCommand"/>.
/// </summary>
public class SendOverclockingToRigsCommandHandler : IRequestHandler<SendOverclockingToRigsCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IQueueBusClient _queueClient;

    public SendOverclockingToRigsCommandHandler(IMapper mapper, IQueueBusClient queueClient)
    {
        _mapper = mapper ??
            throw new ArgumentNullException(nameof(mapper));
        _queueClient = queueClient ??
            throw new ArgumentNullException(nameof(queueClient));
    }

    public async Task<Result<Unit>> Handle(SendOverclockingToRigsCommand request, CancellationToken cancellationToken)
    {
        foreach (var rigDevices in request.Devices.GroupBy(x => x.RigId))
        {
            var rigOverclocking = _mapper.Map<Inventory.Contracts.Devices.Overclocking>(request.Overclocking);

            var command = new Agent.Commands.Overclocking.SetOverclockingCommand(
                rigOverclocking, rigDevices.Select(x => x.Id).ToArray());

            await _queueClient.Enqueue(command, new Guid[] { rigDevices.Key }, request.UserId);
        }
        return Result<Unit>.Empty();
    }
}
