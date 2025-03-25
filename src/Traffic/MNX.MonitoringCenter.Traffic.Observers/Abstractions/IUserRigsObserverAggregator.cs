using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers.Abstractions;

/// <summary>
/// Агрегатор наблюдателей за ригами пользователей.
/// </summary>
public interface IUserRigsObserverAggregator : IDisposable
{
    /// <summary>
    /// Попробовать подписаться на поток показателей.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <param name="onNext"> Обработчик показателей </param>
    /// <returns> Признак успешности подписки. </returns>
    bool TrySubscribe(Guid userId, string subscriberId,
                      SubscriptionType subscriptionType, Action<object> onNext);

    /// <summary>
    /// Отписаться от потока показателей.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <returns> Число подписчиков. </returns>
    void Unsubscribe(Guid userId, string subscriberId, SubscriptionType subscriptionType);

    /// <summary>
    /// Отписаться от всего.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <returns> Число подписчиков. </returns>
    void UnsubscribeFromAll(Guid userId, string subscriberId);

    /// <summary>
    /// Задать показатели.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="rigIndicators"> Показатели рига. </param>
    void SetIndicators(Guid userId, RigDynamicIndicators rigIndicators);
}
