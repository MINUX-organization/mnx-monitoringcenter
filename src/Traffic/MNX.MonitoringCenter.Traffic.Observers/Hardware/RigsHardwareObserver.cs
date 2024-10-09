using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware;

/// <summary>
/// Наблюдатель за аппаратными показателями ригов.
/// </summary>
public class RigsHardwareObserver : BaseRigsObserver<RigDynamicHardwareIndicators,
                                                     IDeviceDynamicHardwareIndicators,
                                                     CpuDynamicHardwareIndicators,
                                                     GpuDynamicHardwareIndicators>
{
    /// <summary>
    /// Поток общей мощности всех ригов.
    /// </summary>
    private Subject<int> _totalPowerStream = new();

    /// <summary>
    /// Счётчик показателей ригов.
    /// </summary>
    private RigsIndicatorsCounter _indicatorsCounter = new();

    /// <inheritdoc/>
    public override (bool IsSuccessful, int SubscriptionsCount) TrySubscribe
        (string subscriberId, SubscriptionType subscriptionType, ChannelWriter<object> writer)
    {
        var subscription = _subscriberSubscriptions.GetValueOrDefault(subscriberId);

        if (subscription is null)
        {
            subscription = new Subscription(subscriberId);
            _subscriberSubscriptions.TryAdd(subscriberId, subscription);
        }

        bool isSuccessful = false;

        if (subscriptionType == SubscriptionType.GeneralHardwareRigsIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, writer, _generalIndicatorsStream);

        else if (subscriptionType == SubscriptionType.CpusHardwareIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, writer, _cpusIndicatorsStream);

        else if (subscriptionType == SubscriptionType.GpusHardwareIndicators)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, writer, _gpusIndicatorsStream);

        else if (subscriptionType == SubscriptionType.TotalPower)
            isSuccessful = TrySubscribeToStream(subscription, subscriptionType, writer, _totalPowerStream);

        return new (isSuccessful, _subscriptionsCount);
    }

    /// <inheritdoc/>
    public override void SetIndicators(IEnumerable<RigDynamicHardwareIndicators> rigsDynamicIndicators)
    {
        base.SetIndicators(rigsDynamicIndicators);
        _indicatorsCounter.UpdateData(rigsDynamicIndicators);
        _totalPowerStream.OnNext(_indicatorsCounter.GetTotalPower());
    }

    /// <inheritdoc/>
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
                _totalPowerStream.Dispose();
                _indicatorsCounter.Dispose();
            }

            _generalIndicatorsStream = null!;
            _cpusIndicatorsStream = null!;
            _gpusIndicatorsStream = null!;
            _totalPowerStream = null!;

            _indicatorsCounter = null!;
            _subscriberSubscriptions = null!;

            _disposedValue = true;
        }
    }

    /// <summary>
    /// Счётчик динамических показателей со всех ригов пользователя.
    /// </summary>
    private class RigsIndicatorsCounter : BaseRigsIndicatorsCounter
    {
        /// <summary>
        /// Получить общую мощность.
        /// </summary>
        /// <returns> Общая мощность. </returns>
        public int GetTotalPower()
        {
            return _rigsIndicators.Sum(rig => rig.Value.TotalPower);
        }
    }
}
