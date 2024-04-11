using EasyNetQ;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using System.Collections.Concurrent;

namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Потокобезопасная реализация <see cref="IUserRigsObserverWrapper"/>
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
    /// Отправитель сообщений в шину.
    /// </summary>
    private readonly IPubSub _pubSub;

    /// <summary>
    /// Наблюдатели за динамическими данными ригов.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор пользователя.
    /// Значение - наблюдатель.
    /// </remarks>
    private ConcurrentDictionary<long, IUserRigsObserver> _observers = new();

    public UserRigsObserverWrapper(IServiceScopeFactory serviceScopeFactory, IPubSub pubSub)
    {
        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));
    }

    /// <inheritdoc/>
    public async Task AddNewSubscriber(long userId, string subscriberId)
    {
        IUserRigsObserver observer;

        try
        {
            observer = GetOrCreateObserver(userId);
            await observer.Subscribe(subscriberId);
        }
        catch(ObjectDisposedException)
        {
            observer = GetOrCreateObserver(userId);
            await observer.Subscribe(subscriberId);
        }
    }

    /// <inheritdoc/>
    public async Task RemoveSubscriber(long userId, string subscriberId)
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
    public void SetObservableCoin(string coin, long userId, string subscriberId)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            observer.SetObservableCoin(subscriberId, coin);
        }
    }

    /// <inheritdoc/>
    public async Task GotDynamicData(long userId, List<RigDynamicData> data)
    {
        if (_observers.TryGetValue(userId, out var observer))
        {
            await observer.GotDynamicData(data);
        }
    }

    /// <inheritdoc/>
    public async Task GotRigsState(long userId, string subscriberId, List<RigState> states)
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
    private IUserRigsObserver GetOrCreateObserver(long userId)
    {
        return _observers.GetOrAdd(userId, x => new UserRigsObserver(_serviceScopeFactory, _pubSub, userId));
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
    private void DisposeObserver(long userId)
    {
        if (_observers.TryRemove(userId, out var observer))
        {
            observer.Dispose();
        }
    }
}
