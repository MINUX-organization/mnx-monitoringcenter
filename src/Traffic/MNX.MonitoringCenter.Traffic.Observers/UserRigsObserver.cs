using AutoMapper;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Mapping;
using MNX.MonitoringCenter.Traffic.Observers.Hardware;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;

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
    /// Подписка на поток показателей аппаратного обеспечения.
    /// </summary>
    private IDisposable _hardwareIndicatorsStreamSubscription;

    /// <summary>
    /// Подписка на поток показателей майнинга.
    /// </summary>
    private IDisposable _miningIndicatorsStreamSubscription;

    /// <summary>
    /// Наблюдатель за аппаратными показателями ригов.
    /// </summary>
    private RigsHardwareObserver _rigsHardwareObserver;

    /// <summary>
    /// Наблюдатель за показателями майнинга ригов.
    /// </summary>
    private RigsMiningObserver _rigsMiningObserver;

    /// <summary>
    /// Скоп сервисов.
    /// </summary>
    private IServiceScope _serviceScope;

    public UserRigsObserver(IServiceScopeFactory serviceScopeFactory,
                            TimeSpan updateIndicatorsPeriod)
    {
        _rigsHardwareObserver = new();
        _rigsMiningObserver = new();

        _rigsIndicatorsStream = new Subject<RigDynamicIndicators>();

        var groupedRigsIndicatorsStream = _rigsIndicatorsStream
            .Buffer(updateIndicatorsPeriod)
            .Where(list => list.Any())
            .Select(list =>
            {
                return list.GroupBy(x => x.RigId)
                           .Select(g => g.OrderByDescending(d => d.SendingDateTime).First())
                           .ToList();
            });

        var hardwareIndicatorsStream = groupedRigsIndicatorsStream
            .Select(item => {
                _serviceScope = serviceScopeFactory.CreateScope();
                return _serviceScope
                .ServiceProvider
                .GetRequiredService<IMapper>()
                .Map<IEnumerable<RigDynamicHardwareIndicators>>(item);
            });

        var miningIndicatorsStream = groupedRigsIndicatorsStream
            .Select(item => {
                _serviceScope = serviceScopeFactory.CreateScope();
                return _serviceScope.ServiceProvider.GetRequiredService<MiningIndicatorsBuilder>().Build(item);
            })
            .Concat();

        _hardwareIndicatorsStreamSubscription = hardwareIndicatorsStream
            .Subscribe(list => _rigsHardwareObserver.SetIndicators(list));

        _miningIndicatorsStreamSubscription = miningIndicatorsStream
            .Subscribe(list => _rigsMiningObserver.SetIndicators(list));
    }

    /// <inheritdoc/>
    public (bool IsSuccessful, int SubscriptionsCount) TrySubscribe
        (string subscriberId, SubscriptionType subscriptionType, Action<object> onNext)
    {
        bool isSuccessful = false;
        var subscriptions = 0;

        if (subscriptionType.IsHardwareSubscription())
        {
            var (IsSuccessful, SubscriptionsCount)
                = _rigsHardwareObserver.TrySubscribe(subscriberId, subscriptionType, onNext);

            isSuccessful = IsSuccessful;
            subscriptions += SubscriptionsCount;
        }
            

        else if (subscriptionType.IsMiningSubscription())
        {
            var (IsSuccessful, SubscriptionsCount)
                = _rigsMiningObserver.TrySubscribe(subscriberId, subscriptionType, onNext);

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
                _hardwareIndicatorsStreamSubscription.Dispose();
                _miningIndicatorsStreamSubscription.Dispose();
                _rigsHardwareObserver.Dispose();
                _rigsMiningObserver.Dispose();
                _serviceScope.Dispose();
            }

            _rigsIndicatorsStream = null!;
            _hardwareIndicatorsStreamSubscription = null!;
            _miningIndicatorsStreamSubscription = null!;
            _rigsHardwareObserver = null!;
            _rigsMiningObserver = null!;
            _serviceScope = null!;

            _disposedValue = true;
        }
    }
}
