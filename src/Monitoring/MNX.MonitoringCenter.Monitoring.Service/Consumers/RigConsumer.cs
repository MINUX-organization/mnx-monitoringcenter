using EasyNetQ.AutoSubscribe;
using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Devices.Gpu.ConfirmOverclocking;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;

namespace MNX.MonitoringCenter.Monitoring.Service.Consumers;

/// <summary>
/// Потребитель сообщений от фермы.
/// </summary>
public class RigConsumer :
    IConsumeAsync<GotRigsStateMessage>,
    IConsumeAsync<GotRigsDynamicData>,
    IConsumeAsync<OverclockingSettingSuccessMessage>,
    IConsumeAsync<OverclockingSettingFailMessage>,
    IConsumeAsync<InventoryMsg>
{
    private readonly IUserRigsObserverWrapper _observer;

    private readonly IMediator _mediator;

    public RigConsumer(IUserRigsObserverWrapper observer, IMediator mediator)
    {
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Получить сообщение состояния ригов.
    /// </summary>
    /// <param name="message"> Сообщение состояния ригов. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(GotRigsStateMessage message, CancellationToken cancellationToken = default)
    {
        return _observer.GotRigsState(message.UserId, message.ConnectionId, message.RigsState);
    }

    /// <summary>
    /// Получить сообщение динамических данных ригов.
    /// </summary>
    /// <param name="message"> Сообщение динамических данных ригов. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public Task ConsumeAsync(GotRigsDynamicData message, CancellationToken cancellationToken = default)
    {
        _observer.GotDynamicData(message.UserId, message.Data);
        return Task.CompletedTask;
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
