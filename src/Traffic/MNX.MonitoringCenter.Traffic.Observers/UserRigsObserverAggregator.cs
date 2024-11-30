using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MNX.MonitoringCenter.Traffic.Contracts.Bus;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using System.Collections.Concurrent;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Реализация <see cref="IUserRigsObserverAggregator"/>.
/// </summary>
public class UserRigsObserverAggregator : IUserRigsObserverAggregator
{
    /// <summary>
    /// Признак утилизированного объекта.
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// Фабрика для создания скопа.
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Наблюдатели за динамическими данными ригов.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор пользователя.
    /// Значение - наблюдатель.
    /// </remarks>
    private ConcurrentDictionary<Guid, IUserRigsObserver> _observers = new();

    /// <summary>
    /// Период обновления динамических показателей в секундах.
    /// </summary>
    private readonly TimeSpan _updateDynamicIndicatorsPeriod;

    public UserRigsObserverAggregator(IServiceScopeFactory serviceScopeFactory,
                                      IOptions<DynamicIndicatorsOptions> options)
    {
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _updateDynamicIndicatorsPeriod = TimeSpan.FromSeconds(options.Value.UpdatePeriodInSeconds);
    }

    /// <inheritdoc/>
    public bool TrySubscribe(Guid userId, string subscriberId,
                             SubscriptionType subscriptionType, Action<object> onNext)
    {
        IUserRigsObserver observer;

        try
        {
            observer = GetOrCreateObserver(userId);
            return observer.TrySubscribe(subscriberId, subscriptionType, onNext).IsSuccessful;
        }
        catch (ObjectDisposedException)
        {
            observer = GetOrCreateObserver(userId);
            return observer.TrySubscribe(subscriberId, subscriptionType, onNext).IsSuccessful;
        }
    }

    /// <inheritdoc/>
    public void Unsubscribe(Guid userId, string subscriberId, SubscriptionType subscriptionType)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            var subscriptionsCount = observer.Unsubscribe(subscriberId, subscriptionType);

            if (subscriptionsCount <= 0)
            {
                DisposeObserver(userId);
            }
        }
    }

    /// <inheritdoc/>
    public void UnsubscribeFromAll(Guid userId, string subscriberId)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            var subscriptionsCount = observer.UnsubscribeFromAll(subscriberId);

            if (subscriptionsCount <= 0)
            {
                DisposeObserver(userId);
            }
        }
    }

    /// <inheritdoc/>
    public void SetIndicators(Guid userId, RigDynamicIndicators rigDynamicIndicators)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            observer.SetIndicators(rigDynamicIndicators);
        }
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
    /// Получить или создать наблюдателя.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    private IUserRigsObserver GetOrCreateObserver(Guid userId)
    {
        // todo: отправить команду на запуск потока показателей на все риги пользователя.
        return _observers.GetOrAdd(userId,
            _ => new UserRigsObserver(_serviceScopeFactory, _updateDynamicIndicatorsPeriod));
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
                foreach (var observer in _observers)
                {
                    DisposeObserver(observer.Key);
                }
            }

            _observers = null!;
            _disposedValue = true;
        }
    }

    /// <summary>
    /// Освободить ресурсы наблюдателя.
    /// </summary>
    private void DisposeObserver(Guid userId)
    {
        if (_observers.TryRemove(userId, out var observer))
        {
            observer.Dispose();
            // todo: отправить команду на остановку потока показателей на все риги пользователя.
        }
    }
}
