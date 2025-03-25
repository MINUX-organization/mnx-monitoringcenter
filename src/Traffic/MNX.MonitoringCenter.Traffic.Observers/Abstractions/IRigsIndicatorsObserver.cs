using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers.Abstractions;

/// <summary>
/// Интерфейс наблюдателя за показателями ригов.
/// </summary>
/// <typeparam name="TRigsIndicators"> Тип показателей ригов. </typeparam>
public interface IRigsIndicatorsObserver<TRigsIndicators> : IDisposable
{
    /// <summary>
    /// Попробовать подписаться на поток показателей.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <param name="writer"> Писатель в канал. </param>
    /// <returns> Признак успешности подписки и число подписок. </returns>
    (bool IsSuccessful, int SubscriptionsCount) TrySubscribe(
        string subscriberId, SubscriptionType subscriptionType, Action<object> onNext);

    /// <summary>
    /// Отписаться от потока показателей.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <returns> Число подписок. </returns>
    int Unsubscribe(string subscriberId, SubscriptionType subscriptionType);

    /// <summary>
    /// Отписаться от всего.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <returns> Число подписок. </returns>
    int UnsubscribeFromAll(string subscriberId);

    /// <summary>
    /// Задать показатели.
    /// </summary>
    /// <param name="rigsIndicators"> Показатели ригов. </param>
    void SetIndicators(TRigsIndicators rigsIndicators);
}
