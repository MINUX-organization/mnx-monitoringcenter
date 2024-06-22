using EasyNetQ.AutoSubscribe;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Messages;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Service.Consumers;

/// <summary>
/// Потребитель сообщений от фермы.
/// </summary>
public class RigConsumer : IConsumeAsync<GotRigsStateMessage>, IConsumeAsync<GotRigsDynamicData>, IConsumeAsync<OverclockingSettingSuccessMessage>, IConsumeAsync<OverclockingSettingFailMessage>
{
    private readonly IUserRigsObserverWrapper _observer;

    public RigConsumer(IUserRigsObserverWrapper observer)
    {
        _observer = observer ?? throw new ArgumentNullException(nameof(observer));
    }

    /// <summary>
    /// Получить сообщение состояния ригов.
    /// </summary>
    /// <param name="message"> Сообщение состояния ригов. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public async Task ConsumeAsync(GotRigsStateMessage message, CancellationToken cancellationToken = default)
    {
        await _observer.GotRigsState(message.UserId, message.ConnectionId, message.RigsState);
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
    public async Task ConsumeAsync(OverclockingSettingSuccessMessage message, CancellationToken cancellationToken = default)
    {
        await _observer.GotOverclockingSettingSuccess(message.CardId, message.Overclocking, message.UserId, message.ConnectionId);
    }

    /// <summary>
    /// Получить сообщение о неудачной установке разгона.
    /// </summary>
    /// <param name="message"> Сообщение о неудачной установке разгона. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    public async Task ConsumeAsync(OverclockingSettingFailMessage message, CancellationToken cancellationToken = default)
    {
        await _observer.GotOverclockingSettingFail(message.CardId, message.Message, message.UserId, message.ConnectionId);
    }   
}
