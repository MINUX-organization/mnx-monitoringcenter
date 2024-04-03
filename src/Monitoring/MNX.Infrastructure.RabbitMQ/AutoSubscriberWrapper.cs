using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using EasyNetQ.DI;

namespace MNX.Infrastructure.RabbitMQ;

/// <summary>
/// Обёртка над <see cref="AutoSubscriber"/>.
/// </summary>
internal class AutoSubscriberWrapper
{
    /// <summary>
    /// Сервис, предоставляющий экземпляры запрашиваемых сервисов.
    /// </summary>
    private readonly IServiceResolver _serviceResolver;

    /// <summary>
    /// Массив потребителей.
    /// </summary>
    private readonly Type[] _consumers;

    /// <summary>
    /// Префикс идентифкатора подписки.
    /// </summary>
    private readonly string _subscriptionIdPrefix;

    /// <summary>
    /// Создаёт экземпляр класса <see cref="AutoSubscriberWrapper"/>.
    /// </summary>
    /// <param name="serviceResolver"> Сервис, предоставляющий экземпляры запрашиваемых сервисов. </param>
    /// <param name="consumers"> Массив потребителей. </param>
    /// <param name="subscriptionIdPrefix"> Префикс идентифкатора подписки. </param>
    internal AutoSubscriberWrapper(IServiceResolver serviceResolver, Type[] consumers, string subscriptionIdPrefix)
    {
        _serviceResolver = serviceResolver;
        _consumers = consumers;
        _subscriptionIdPrefix = subscriptionIdPrefix;
    }

    /// <summary>
    /// Подписаться на очереди.
    /// </summary>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns> Подписка. </returns>
    internal async Task<IDisposable> SubscribeAsync(CancellationToken cancellationToken = default)
    {
        var autoSubscriber = new AutoSubscriber(_serviceResolver.Resolve<IBus>(), _subscriptionIdPrefix)
        {
            AutoSubscriberMessageDispatcher = new DefaultAutoSubscriberMessageDispatcher(_serviceResolver),
            GenerateSubscriptionId = x => x.MessageType.Name.Split('.').Last(),
            ConfigureSubscriptionConfiguration = c => c.WithAutoDelete()
        };

        return await autoSubscriber.SubscribeAsync(_consumers, cancellationToken).ConfigureAwait(false);
    }
}
