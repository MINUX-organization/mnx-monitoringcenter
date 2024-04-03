using Microsoft.Extensions.Hosting;

namespace MNX.Infrastructure.RabbitMQ;

/// <summary>
/// Сервис, запускающийся в фоновом процессе для подписки на очереди.
/// </summary>
/// <typeparam name="T"> Тип подписчика. </typeparam>
internal class BusSubscriber<T> : IHostedService where T : AutoSubscriberWrapper
{
    /// <summary>
    /// Подписчик.
    /// </summary>
    private readonly T _subscriber;

    /// <summary>
    /// Подписки.
    /// </summary>
    private IDisposable? _subscriptions;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="BusSubscriber{T}"/>.
    /// </summary>
    /// <param name="subscriber"> Подписчик. </param>
    public BusSubscriber(T subscriber)
    {
        _subscriber = subscriber;
    }

    /// <inheritdoc/>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _subscriptions = await _subscriber.SubscribeAsync(cancellationToken).ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task StopAsync(CancellationToken cancellationToken)
    {
        if (_subscriptions is IAsyncDisposable asyncDispose)
        {
            await asyncDispose.DisposeAsync().ConfigureAwait(false);
        }

        _subscriptions?.Dispose();
    }
}
