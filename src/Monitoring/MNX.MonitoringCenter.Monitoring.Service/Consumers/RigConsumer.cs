using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.ConfirmOverclocking;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.Consumers;

/// <summary>
/// Потребитель сообщений от фермы.
/// </summary>
public class RigConsumer :
    IConsumeAsync<OverclockingSettingSuccessMessage>,
    IConsumeAsync<OverclockingSettingFailMessage>,
    IConsumeAsync<InventoryMsg>
{
    private readonly IMediator _mediator;

    public RigConsumer(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить сообщение об удачной установке разгона.
    /// </summary>
    /// <param name="message"> Сообщение об удачной установке разгона. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(OverclockingSettingSuccessMessage message,
                                   CancellationToken cancellationToken = default)
    {
        return _mediator.Send(
            new GpuOverclockingConfirmationCommand(message.CardId, message.Overclocking),
                                                   cancellationToken);
    }

    /// <summary>
    /// Получить сообщение о неудачной установке разгона.
    /// </summary>
    /// <param name="message"> Сообщение о неудачной установке разгона. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(OverclockingSettingFailMessage message,
                                   CancellationToken cancellationToken = default)
    {
        return _mediator.Publish(new OverclockingSettingFailEvent(
            message.ConnectionId, message.CardId, new string[] { message.Message }), cancellationToken);
    }

    /// <summary>
    /// Получить сообщение с инвентаризацией.
    /// </summary>
    /// <param name="message"> Сообщение с инвентаризацией. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns></returns>
    public Task ConsumeAsync(InventoryMsg message, CancellationToken cancellationToken = default)
    {
        return _mediator.Send(new SaveInventoryCommand(message), cancellationToken);
    }
}
