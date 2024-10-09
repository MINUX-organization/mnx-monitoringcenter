using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Реализация <see cref="IUserRigsObserver"/>.
/// </summary>
public class UserRigsObserver : IUserRigsObserver
{
    /// <summary>
    /// Признак утилизированного объекта.
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// Поток динамических показателей с ригов.
    /// </summary>
    private Subject<RigDynamicIndicators> _rigsIndicatorsStream;

    /// <summary>
    /// Поток показателей, сгруппированных по ригам.
    /// </summary>
    private IObservable<IEnumerable<RigDynamicIndicators>> _groupedRigsIndicatorsStream;

    /// <summary>
    /// Подписка на поток показателей, сгруппированных по ригам.
    /// </summary>
    private IDisposable _groupedRigsIndicatorsStreamSubscription;

    /// <summary>
    /// Наблюдатель за аппаратными показателями ригов.
    /// </summary>
    private RigsHardwareObserver _rigsHardwareObserver;

    /// <summary>
    /// Наблюдатель за показателями майнинга ригов.
    /// </summary>
    private RigsMiningObserver _rigsMiningObserver;

    public UserRigsObserver(IServiceScopeFactory serviceScopeFactory,
                            TimeSpan updateIndicatorsPeriod)
    {
        _rigsHardwareObserver = new();
        _rigsMiningObserver = new();

        _rigsIndicatorsStream = new Subject<RigDynamicIndicators>();

        using var scope = serviceScopeFactory.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        _groupedRigsIndicatorsStream = _rigsIndicatorsStream
            .Buffer(updateIndicatorsPeriod)
            .Where(list => list.Any())
            .Select(list =>
            {
                return list.GroupBy(x => x.RigId)
                           .Select(g => g.OrderByDescending(d => d.SendingDateTime).First())
                           .ToList();
            });

        _groupedRigsIndicatorsStreamSubscription = _groupedRigsIndicatorsStream.Subscribe(list =>
        {
            _rigsHardwareObserver.SetIndicators(mapper.Map<IEnumerable<RigDynamicHardwareIndicators>>(list));
            _rigsMiningObserver.SetIndicators(mapper.Map<IEnumerable<RigDynamicMiningIndicators>>(list));
        });
    }

    /// <inheritdoc/>
    public (bool IsSuccessful, int SubscriptionsCount) TrySubscribe
        (string subscriberId, SubscriptionType subscriptionType, ChannelWriter<object> writer)
    {
        bool isSuccessful = false;
        var subscriptions = 0;

        if (subscriptionType.IsHardwareSubscription())
        {
            var (IsSuccessful, SubscriptionsCount)
                = _rigsHardwareObserver.TrySubscribe(subscriberId, subscriptionType, writer);

            isSuccessful = IsSuccessful;
            subscriptions += SubscriptionsCount;
        }
            

        else if (subscriptionType.IsMiningSubscription())
        {
            var (IsSuccessful, SubscriptionsCount)
                = _rigsMiningObserver.TrySubscribe(subscriberId, subscriptionType, writer);

            isSuccessful = IsSuccessful;
            subscriptions += SubscriptionsCount;
        }
            

        return new (isSuccessful, subscriptions);
    }

    /// <inheritdoc/>
    public int Unsubscribe(string subscriberId, SubscriptionType subscriptionType)
    {
        var subscriptions = 0;

        if (subscriptionType.IsHardwareSubscription())
            subscriptions += _rigsHardwareObserver.Unsubscribe(subscriberId, subscriptionType);

        else if (subscriptionType.IsMiningSubscription())
            subscriptions += _rigsMiningObserver.Unsubscribe(subscriberId, subscriptionType);

        return subscriptions;
    }

    /// <inheritdoc/>
    public int UnsubscribeFromAll(string subscriberId)
    {
        var subscriptions = 0;

        subscriptions += _rigsHardwareObserver.UnsubscribeFromAll(subscriberId);
        subscriptions += _rigsMiningObserver.UnsubscribeFromAll(subscriberId);

        return subscriptions;
    }

    /// <inheritdoc/>
    public void SetIndicators(RigDynamicIndicators rigsDynamicIndicators)
    {
        _rigsIndicatorsStream.OnNext(rigsDynamicIndicators);
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Освободить ресурсы.
    /// </summary>
    private void Dispose(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing)
            {
                _groupedRigsIndicatorsStreamSubscription.Dispose();
                _rigsHardwareObserver.Dispose();
                _rigsMiningObserver.Dispose();
            }

            _rigsIndicatorsStream = null!;
            _groupedRigsIndicatorsStream = null!;
            _groupedRigsIndicatorsStreamSubscription = null!;
            _rigsHardwareObserver = null!;
            _rigsMiningObserver = null!;

            _disposedValue = true;
        }
    }
}
