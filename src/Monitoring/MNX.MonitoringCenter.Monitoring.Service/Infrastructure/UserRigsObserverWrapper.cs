using Microsoft.Extensions.Options;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using System.Collections.Concurrent;
using ZiggyCreatures.Caching.Fusion;

namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Потокобезопасная реализация <see cref="IUserRigsObserverWrapper"/>.
/// </summary>
public class UserRigsObserverWrapper : IUserRigsObserverWrapper
{
    /// <summary>
    /// Признак утилизированного объекта.
    /// </summary>
    private bool _disposedValue;

    /// <summary>
    /// Фабрика для создания DI.
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
    /// Период обновления динамических данных.
    /// </summary>
    private readonly DynamicDataOptions _updateDynamicDataPeriod;

    /// <summary>
    /// Кеш ригов.
    /// </summary>
    private readonly IFusionCache _rigsCache;

    public UserRigsObserverWrapper(IServiceScopeFactory serviceScopeFactory,
                                   IOptions<DynamicDataOptions> options,
                                   IFusionCache cache)
    {
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory)); 

        _updateDynamicDataPeriod = options.Value ?? throw new ArgumentNullException(nameof(options));
        _rigsCache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    /// <inheritdoc/>
    public async Task AddNewSubscriber(Guid userId, string subscriberId, bool subscribeToDynamicDataStream)
    {
        IUserRigsObserver observer;

        try
        {
            observer = GetOrCreateObserver(userId);
            await observer.Subscribe(subscriberId, subscribeToDynamicDataStream);
        }
        catch(ObjectDisposedException)
        {
            observer = GetOrCreateObserver(userId);
            await observer.Subscribe(subscriberId, subscribeToDynamicDataStream);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveSubscriber(Guid userId, string subscriberId)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            var subscribersCount = await observer.Unsubscribe(subscriberId);

            if (subscribersCount <= 0)
            {
                DisposeObserver(userId);
            }
        }
    }

    /// <inheritdoc/>
    public async Task SetObservableCoin(string coin, Guid userId, string subscriberId)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            await observer.SetObservableCoin(subscriberId, coin);
        }
    }

    /// <inheritdoc/>
    public void SetSearchString(string searchString, Guid userId, string subscriberId)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            observer.SetSearchString(subscriberId, searchString);
        }
    }

    /// <inheritdoc/>
    public void GotDynamicData(Guid userId, List<RigDynamicData> data)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            observer.GotDynamicData(data);
        }
    }

    /// <inheritdoc/>
    public async Task GotRigsState(Guid userId, string subscriberId, List<RigState> states)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            await observer.GotRigsState(subscriberId, states);
        }
    }

    /// <summary>
    /// Получить или создать наблюдателя.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    private IUserRigsObserver GetOrCreateObserver(Guid userId)
    {
        return _observers.GetOrAdd(userId, x => new UserRigsObserver(_serviceScopeFactory,
                                                                     userId,
                                                                     _updateDynamicDataPeriod,
                                                                     _rigsCache));
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
    protected virtual void Dispose(bool disposing)
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
        }
    }
}
