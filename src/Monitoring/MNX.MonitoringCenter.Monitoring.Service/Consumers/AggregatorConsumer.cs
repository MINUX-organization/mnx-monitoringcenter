using EasyNetQ.AutoSubscribe;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages.Bus;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Service.Consumers;

/// <summary>
/// Потребитель сообщений от фермы.
/// </summary>
public class AggregatorConsumer : IConsumeAsync<GotRigsStateMessage>, IConsumeAsync<GotRigsDynamicData>
{
    private readonly IUserRigsObserverWrapper _observer;

    public AggregatorConsumer(IUserRigsObserverWrapper observer)
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
}
