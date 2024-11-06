using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory;

namespace MNX.MonitoringCenter.Inventory.Controllers;

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
        return _mediator.Send(new SaveRigInventoryCommand(message), cancellationToken);
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
