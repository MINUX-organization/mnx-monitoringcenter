using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.Traffic.Observers.Abstractions;

/// <summary>
/// Базовый наблюдатель за ригами.
/// </summary>
/// <typeparam name="TRigsIndicators"> Тип показателей ригов. </typeparam>
/// <typeparam name="TDeviceIndicators"> Тип показателей устройств. </typeparam>
/// <typeparam name="TCpusIndicators"> Тип показателей процессоров. </typeparam>
/// <typeparam name="TGpusIndicators"> Тип показателей видеокарт. </typeparam>
public abstract class BaseRigsObserver<TRigsIndicators, TDeviceIndicators, TCpusIndicators, TGpusIndicators>
        : IRigsIndicatorsObserver<IEnumerable<TRigsIndicators>>
                                      where TRigsIndicators : IRigIndicators<TDeviceIndicators>
                                      where TDeviceIndicators : IDeviceIndicators
                                      where TCpusIndicators : TDeviceIndicators
                                      where TGpusIndicators : TDeviceIndicators
{
    /// <summary>
    /// Признак утилизированного объекта.
    /// </summary>
    protected bool _disposedValue;

    /// <summary>
    /// Количество подписок.
    /// </summary>
    protected int _subscriptionsCount;

    /// <summary>
    /// Поток динамических показателей с ригов.
    /// </summary>
    protected Subject<IEnumerable<TRigsIndicators>> _generalIndicatorsStream;
    /// <summary>
    /// Поток динамических показателей процессоров.
    /// </summary>
    protected IObservable<IEnumerable<TCpusIndicators>> _cpusIndicatorsStream;
    /// <summary>
    /// Поток динамических показателей видеокарт.
    /// </summary>
    protected IObservable<IEnumerable<TGpusIndicators>> _gpusIndicatorsStream;

    /// <summary>
    /// Подписки каждого подписчика.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор подписчика.
    /// Значение - подписка.
    /// </remarks>
    protected ConcurrentDictionary<string, Subscription> _subscriberSubscriptions = new();

    public BaseRigsObserver()
    {
        _generalIndicatorsStream = new Subject<IEnumerable<TRigsIndicators>>();

        _cpusIndicatorsStream = _generalIndicatorsStream.Select(rigs =>
            rigs.SelectMany(rig => rig.Devices.Where(device => device.Type == DeviceType.CPU)
                                              .Select(device => (TCpusIndicators)device)));

        _gpusIndicatorsStream = _generalIndicatorsStream.Select(rigs =>
            rigs.SelectMany(rig => rig.Devices.Where(device => device.Type == DeviceType.GPU)
                                              .Select(device => (TGpusIndicators)device)));
    }

    /// <inheritdoc/>
    public abstract (bool IsSuccessful, int SubscriptionsCount) TrySubscribe(
        string subscriberId, SubscriptionType subscriptionType, ChannelWriter<object> writer);

    /// <inheritdoc/>
    public virtual int Unsubscribe(string subscriberId, SubscriptionType subscriptionType)
    {
        var subscription = _subscriberSubscriptions.GetValueOrDefault(subscriberId);

        if (subscription is null)
        {
            return _subscriptionsCount;
        }

        if (subscription.TryRemove(subscriptionType))
        {
            Interlocked.Decrement(ref _subscriptionsCount);
        }

        return _subscriptionsCount;
    }

    /// <inheritdoc/>
    public virtual int UnsubscribeFromAll(string subscriberId)
    {
        var subscription = _subscriberSubscriptions.GetValueOrDefault(subscriberId);

        if (subscription is null)
        {
            return _subscriptionsCount;
        }

        var subscriptionCount = subscription.StreamSubscriptions.Count;
        subscription.Dispose();

        return Interlocked.Add(ref _subscriptionsCount, - subscriptionCount);
    }

    /// <inheritdoc/>
    public virtual void SetIndicators(IEnumerable<TRigsIndicators> rigsDynamicIndicators)
    {
        _generalIndicatorsStream.OnNext(rigsDynamicIndicators);
    }

    /// <summary>
    /// Попробовать подписаться на поток показателей.
    /// </summary>
    /// <typeparam name="TIndicators"> Тип показателей. </typeparam>
    /// <param name="subscription"> Подписка. </param>
    /// <param name="subscriptionType"> Тип подписки. </param>
    /// <param name="writer"> Писатель в канал. </param>
    /// <param name="stream"> Поток показателей. </param>
    /// <returns> Признак успешной подписки. </returns>
    protected bool TrySubscribeToStream<TIndicators>(Subscription subscription, SubscriptionType subscriptionType,
                                                     ChannelWriter<object> writer, IObservable<TIndicators> stream)
    {
        var streamSubscription = new Subscription.StreamSubscription<object>(
                                        subscriptionType,
                                        stream.Subscribe(async data => await writer.WriteAsync(data!)),
                                        writer);

        if (subscription.TryAdd(streamSubscription))
        {
            Interlocked.Increment(ref _subscriptionsCount);
            return true;
        }

        streamSubscription.Dispose();
        return false;
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
    protected abstract void Dispose(bool disposing);

    /// <summary>
    /// Подписка.
    /// </summary>
    protected class Subscription : IDisposable
    {
        /// <summary>
        /// Признак утилизированного объекта.
        /// </summary>
        protected bool _disposedValue;

        /// <summary>
        /// Идентификатор подписчика.
        /// </summary>
        public string SubscriberId { get; }

        /// <summary>
        /// Подписки на потоки показателей.
        /// </summary>
        public ConcurrentDictionary<SubscriptionType, StreamSubscription<object>> StreamSubscriptions { get; private set; }

        public Subscription(string subscriberId)
        {
            SubscriberId = subscriberId;
            StreamSubscriptions = new();
        }

        /// <summary>
        /// Попробовать добавить подписку на поток.
        /// </summary>
        /// <param name="subscription">Подписка на поток. </param>
        /// <returns> Успешность добавления подписки. </returns>
        public bool TryAdd(StreamSubscription<object> subscription)
        {
            return StreamSubscriptions.TryAdd(subscription.Type, subscription);
        }

        /// <summary>
        /// Попробовать удалить подписку на поток.
        /// </summary>
        /// <param name="subscription"> Подписка на поток. </param>
        /// <returns> Успешность удаления подписки. </returns>
        public bool TryRemove(SubscriptionType subscription)
        {
            if (StreamSubscriptions.TryRemove(subscription, out StreamSubscription<object> removedSubscription))
            {
                removedSubscription.Dispose();
                return true;
            }

            return false;
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
                    foreach (var subscription in StreamSubscriptions)
                    {
                        if (StreamSubscriptions.TryRemove(subscription.Key, out var removedSub))
                        {
                            removedSub.Dispose();
                        }
                    }
                }

                StreamSubscriptions = null!;

                _disposedValue = true;
            }
        }

        /// <summary>
        /// Подписка на поток показателей.
        /// </summary>
        /// <typeparam name="TIndicators"> Тип показателей. </typeparam>
        public struct StreamSubscription<TIndicators> : IDisposable where TIndicators : class
        {
            /// <summary>
            /// Тип подписки.
            /// </summary>
            public SubscriptionType Type { get; }

            /// <summary>
            /// Значение подписки.
            /// </summary>
            public IDisposable Value { get; }

            /// <summary>
            /// Спецификация подписки.
            /// </summary>
            public Specification Specification { get; set; }

            /// <summary>
            /// Писатель в канал.
            /// </summary>
            public ChannelWriter<TIndicators> Writer { get; }

            public StreamSubscription(SubscriptionType type, IDisposable value, ChannelWriter<TIndicators> channelWriter)
            {
                Type = type;
                Value = value;
                Specification = new Specification();
                Writer = channelWriter;
            }

            /// <summary>
            /// Освободить ресурсы.
            /// </summary>
            public readonly void Dispose()
            {
                Value.Dispose();
                Writer.TryComplete();
            }
        }
    }

    /// <summary>
    /// Спецификация.
    /// </summary>
    protected struct Specification
    {
        /// <summary>
        /// Строка поиска.
        /// </summary>
        public string? SearchString { get; set; }

        /// <summary>
        /// Строка для фильтрации.
        /// </summary>
        public string? FilterString { get; private set; }

        /// <summary>
        /// Аргументы для строки фильтрации.
        /// </summary>
        public string[]? FilterArguments { get; private set; }

        /// <summary>
        /// Задать фильтр.
        /// </summary>
        /// <param name="filterString"> Строка для фильтрации. </param>
        /// <param name="filterArguments"> Аргументы для строки фильтрации. </param>
        public void SetFilter(string filterString, string[] filterArguments)
        {
            FilterString = filterString;
            FilterArguments = filterArguments;
        }

        /// <summary>
        /// Сбросить фильтр.
        /// </summary>
        public void ResetFilter()
        {
            FilterString = null;
            FilterArguments = null;
        }
    }

    /// <summary>
    /// Счётчик показателей со всех ригов пользователя.
    /// </summary>
    protected class BaseRigsIndicatorsCounter : IDisposable
    {
        /// <summary>
        /// Признак утилизированного объекта.
        /// </summary>
        private bool _disposedValue;

        /// <summary>
        /// Показатели ригов.
        /// </summary>
        /// <remarks>
        /// Ключ - идентификатор рига.
        /// Значение - динамические показатели рига.
        /// </remarks>
        protected ConcurrentDictionary<Guid, TRigsIndicators> _rigsIndicators = new();

        /// <summary>
        /// Обновить данные.
        /// </summary>
        /// <param name="rigsIndicators"> Показатели ригов. </param>
        public void UpdateData(IEnumerable<TRigsIndicators> rigsIndicators)
        {
            foreach (var rig in rigsIndicators)
            {
                _rigsIndicators.AddOrUpdate(rig.RigId, rig, (_, value) => value = rig);
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
        /// Освободить ресурсы.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    foreach (var rigData in _rigsIndicators)
                    {
                        _rigsIndicators.TryRemove(rigData.Key, out var _);
                    }
                }

                _rigsIndicators = null!;
                _disposedValue = true;
            }
        }
    }
}
