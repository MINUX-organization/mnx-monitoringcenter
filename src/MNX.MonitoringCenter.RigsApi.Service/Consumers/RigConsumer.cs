using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetRigDevices;

namespace MNX.MonitoringCenter.RigsApi.Service.Consumers;

/// <summary>
/// Потребитель сообщений от ригов.
/// </summary>
public class RigConsumer : IConsumeAsync<RigInventoryMsg>, IConsumeAsync<RigRegisteredMsg>
{
    private readonly IMediator _mediator;

    public RigConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить сообщение с инвентаризацией.
    /// </summary>
    /// <param name="message"> Сообщение с инвентаризацией. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(RigInventoryMsg message, CancellationToken cancellationToken = default)
    {
        return Task.WhenAll(

            _mediator.Send(new SaveRigInventoryCommand(message), cancellationToken),

            _mediator.Send(
                new SetRigDevicesCommand(message.RigId, message.RigOwnerId,
                                         message.Inventory.Gpus, message.Inventory.Cpus),
                cancellationToken)
        );
    }

    /// <summary>
    /// Получить сообщение о регистрации рига.
    /// </summary>
    /// <param name="message"> Сообщение о регистрации рига. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(RigRegisteredMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new AddRigCommand(message.RigId, message.OwnerId), cancellationToken);
    }
}

