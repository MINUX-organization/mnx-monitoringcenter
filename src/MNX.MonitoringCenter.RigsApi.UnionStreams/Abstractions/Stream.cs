using System.Reactive.Subjects;
using MNX.MonitoringCenter.Traffic.Observers;
using Microsoft.Extensions.DependencyInjection;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

public abstract class Stream
{
    /// <summary>
    /// Типы подписок.
    /// </summary>
    protected abstract SubscriptionType[] SubscriptionTypes { get; }

    /// <summary>
    /// Поток данных субъекта.
    /// </summary>
    protected Subject<(SubscriptionType, object)> Subject { get; private set; } = new();

    //TODO: config time span
    /// <summary>
    /// Время ожидания.
    /// </summary>
    protected TimeSpan TimeSpan { get; set; } = TimeSpan.FromSeconds(10);

    /// <summary>
    /// Скоуп фабрика.
    /// </summary>
    protected IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Запуск потока.
    /// </summary>
    /// <returns> Поток данных. </returns>
    public abstract IAsyncEnumerable<object> StartStreaming();

    /// <summary>
    /// Остановка потока.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="connectionId"> Идентификатор соединения. </param>
    public abstract void StopStreaming(Guid userId, string connectionId);
}
