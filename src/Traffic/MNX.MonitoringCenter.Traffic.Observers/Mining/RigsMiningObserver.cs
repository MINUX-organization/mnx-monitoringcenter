using System.Reactive.Linq;
using System.Reactive.Subjects;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining;

/// <summary>
/// Наблюдатель за показателями майнинга ригов.
/// </summary>
public class RigsMiningObserver : BaseRigsObserver<RigDynamicMiningIndicators,
                                                   IDeviceDynamicMiningIndicators,
                                                   CpuDynamicMiningIndicators,
                                                   GpuDynamicMiningIndicators>
{
    /// <summary>
    /// Поток общих решений майнинга.
    /// </summary>
    private Subject<SharesModel> _totalSharesStream = new();

    /// <summary>
    /// Поток общей скорости хеширования.
    /// </summary>
    private Subject<int> _totalHashRateStream = new();

    /// <summary>
    /// Поток общей статистки майнинга, сгруппированной по монетам.
    /// </summary>
    private Subject<IEnumerable<CoinStatistics>> _totalCoinsStatisticsStream = new();

    /// <summary>
    /// Счётчик показателей ригов.
    /// </summary>
    private RigsIndicatorsCounter _indicatorsCounter = new();

    /// <inheritdoc/>
    public override (bool IsSuccessful, int SubscriptionsCount) TrySubscribe
        (string subscriberId, SubscriptionType subscriptionType, Action<object> onNext)
    {
        var subscription = _subscriberSubscriptions.GetValueOrDefault(subscriberId);

        if (subscription is null)
        {
            subscription = new Subscription(subscriberId);
            _subscriberSubscriptions.TryAdd(subscriberId, subscription);
        }

        bool isSuccessful = false;

        if (subscriptionType == SubscriptionType.GeneralMiningRigsIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _generalIndicatorsStream);

        else if (subscriptionType == SubscriptionType.CpusMiningIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _cpusIndicatorsStream);

        else if (subscriptionType == SubscriptionType.GpusMiningIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _gpusIndicatorsStream);

        else if (subscriptionType == SubscriptionType.TotalShares)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _totalSharesStream);

        else if (subscriptionType == SubscriptionType.TotalHashRate)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _totalHashRateStream);

        else if (subscriptionType == SubscriptionType.TotalCoinsStatistics)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, onNext, _totalCoinsStatisticsStream);

        return new(isSuccessful, _subscriptionsCount);
    }

    /// <inheritdoc/>
    public override void SetIndicators(IEnumerable<RigDynamicMiningIndicators> rigsDynamicIndicators)
    {
        base.SetIndicators(rigsDynamicIndicators);
        _indicatorsCounter.UpdateData(rigsDynamicIndicators);
        _totalSharesStream.OnNext(_indicatorsCounter.GetTotalShares());
        _totalHashRateStream.OnNext(_indicatorsCounter.GetTotalHashRate());
        _totalCoinsStatisticsStream.OnNext(_indicatorsCounter.GetTotalCoinStatistics());
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                foreach (var subscriber in _subscriberSubscriptions)
                {
                    if (_subscriberSubscriptions.TryRemove(subscriber.Key, out var subscription))
                    {
                        subscription.Dispose();
                    }
                }

                _generalIndicatorsStream.Dispose();
                _totalSharesStream.Dispose();
                _totalHashRateStream.Dispose();
                _totalCoinsStatisticsStream.Dispose();

                _indicatorsCounter.Dispose();
            }

            _generalIndicatorsStream = null!;
            _cpusIndicatorsStream = null!;
            _gpusIndicatorsStream = null!;
            _totalSharesStream = null!;
            _totalHashRateStream = null!;
            _totalCoinsStatisticsStream = null!;

            _indicatorsCounter = null!;
            _subscriberSubscriptions = null!;

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Счётчик показателей ригов.
    /// </summary>
    private class RigsIndicatorsCounter : BaseRigsIndicatorsCounter
    {
        /// <summary>
        /// Получить общие решения.
        /// </summary>
        /// <returns> Общие решения. </returns>
        public SharesModel GetTotalShares()
        {
            var totalShares = new SharesModel();

            foreach (var rigIndicators in _rigsIndicators.Values)
            {
                totalShares += rigIndicators.TotalShares;
            }

            return totalShares;
        }

        /// <summary>
        /// Получить общую скорость хеширования.
        /// </summary>
        /// <returns></returns>
        public int GetTotalHashRate()
        {
            return _rigsIndicators.Sum(rig => rig.Value.TotalHashRate);
        }

        /// <summary>
        /// Получить общую статистику по монетам.
        /// </summary>
        /// <returns> Общая статистика по монетам. </returns>
        public List<CoinStatistics> GetTotalCoinStatistics()
        {
            var coinsStatistics = new Dictionary<Guid, CoinStatistics>(); // ключ - идентификатор монеты

            foreach (var rigIndicators in _rigsIndicators.Values)
            {
                foreach (var coin in rigIndicators.TotalCoinStatistics)
                {
                    if (coinsStatistics.TryGetValue(coin.CoinId, out CoinStatistics? statistics))
                    {
                        statistics.HashRate += coin.HashRate;
                        statistics.Shares += coin.Shares;
                    }
                    else
                    {
                        coinsStatistics.Add(coin.CoinId, coin);
                    }
                }
            }

            return coinsStatistics.Values.ToList();
        }
    }
}
