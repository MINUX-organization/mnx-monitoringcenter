using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateDynamicData;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateTotalDynamicData;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ZiggyCreatures.Caching.Fusion;

namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

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
    /// Фабрика для создания DI.
    /// </summary>
    private readonly IServiceScopeFactory _serviceScopeFactory;

    /// <summary>
    /// Количество подписчиков.
    /// </summary>
    private long _subscribersCount;

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    private readonly Guid _userId;

    /// <summary>
    /// Поток динамических данных с ригов.
    /// </summary>
    private ISubject<List<RigDynamicData>> _rigsDynamicDataStream;

    /// <summary>
    /// Подписки на поток динамических данных с ригов.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор подписчика.
    /// Значение - Подписка на поток динамических данных с ригов.
    /// </remarks>
    private ConcurrentDictionary<string, IDisposable> _rigsDynamicDataStreamSubscriptions = new();

    /// <summary>
    /// Спецификации фильтрации данных для каждого подписчика.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор подписчика.
    /// Значение - спецификация.
    /// </remarks>
    private ConcurrentDictionary<string, RigsDataSpecification> _subscriberSpecifications = new();

    /// <summary>
    /// Счётчик динамических данных ригов.
    /// </summary>
    private UserRigsDynamicDataCounter _rigsDynamicDataCounter;

    /// <summary>
    /// Кеш состояния ригов.
    /// </summary>
    private readonly IFusionCache _rigsStateCache;

    /// <summary>
    /// Параметры динамических данных.
    /// </summary>
    private DynamicDataOptions _dynamicDataOptions;

    public UserRigsObserver(IServiceScopeFactory serviceScopeFactory,
                            Guid userId,
                            DynamicDataOptions dynamicDataOptions,
                            IFusionCache cache)
    {
        _rigsDynamicDataCounter = new UserRigsDynamicDataCounter(userId, dynamicDataOptions);
        _rigsStateCache = cache ?? throw new ArgumentNullException(nameof(cache));
        _dynamicDataOptions = dynamicDataOptions ?? throw new ArgumentNullException(nameof(dynamicDataOptions));

        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _userId = userId;

        _rigsDynamicDataStream = new Subject<List<RigDynamicData>>();
    }

    /// <inheritdoc/>
    public async Task<long> Subscribe(string subscriberId, bool subscribeToDynamicDataStream)
    {
        if (!_subscriberSpecifications.TryAdd(subscriberId, new RigsDataSpecification()))
        {
            return _subscribersCount;
        }

        var mediator = GetMediator();

        await SendRigsState(mediator, subscriberId);

        if (subscribeToDynamicDataStream)
        {
            await SubscribeToDynamicDataStream(mediator, subscriberId);
        }

        return Interlocked.Increment(ref _subscribersCount);
    }

    /// <inheritdoc/>
    public async Task<long> Unsubscribe(string subscriberId)
    {
        if (!_subscriberSpecifications.TryRemove(subscriberId, out var _))
        {
            return _subscribersCount;
        }

        if (_rigsDynamicDataStreamSubscriptions.TryRemove(subscriberId, out var subscription))
        {
            subscription.Dispose();

            if (_rigsDynamicDataStreamSubscriptions.IsEmpty)
            {
                var mediator = GetMediator();
                await mediator.Publish(new DynamicDataStreamStoppingEvent(_userId));
            }
        }

        return Interlocked.Decrement(ref _subscribersCount);
    }

    /// <inheritdoc/>
    public async Task SetObservableCoin(string subscriberId, string coin)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        _subscriberSpecifications.AddOrUpdate(subscriberId,
                                    new RigsDataSpecification() { ObservableCoin = coin },
                                    (_, value) =>
                                    {
                                        value.ObservableCoin = coin;
                                        return value;
                                    });

        var hashRateHistory = _rigsDynamicDataCounter
                                .GetTotalHashRateHistory(_subscriberSpecifications[subscriberId])
                                .Select(x => new HashRateModel()
                                {
                                    Time = x.Item1,
                                    Value = mapper.Map<ParameterModelWithMeasureUnit>(x.Item2, options =>
                                    {
                                        // todo : вынести в конфиги
                                        options.Items.Add("DefaultMeasureUnit", "H/s");
                                        options.Items.Add("MeasureUnits", new string[] { "H/s", "KH/s", "MH/s", "TH/s" });
                                    })
                                })
                                .ToList();

        await mediator.Publish(new SubscriberAwaitHashRateHistory(subscriberId, hashRateHistory));
    }

    /// <inheritdoc/>
    public void SetSearchString(string subscriberId, string searchString)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        _subscriberSpecifications.AddOrUpdate(subscriberId,
                                    new RigsDataSpecification() { RigsSearchString = searchString },
                                    (_, value) =>
                                    {
                                        value.RigsSearchString = searchString;
                                        return value;
                                    });
    }

    /// <inheritdoc/>
    public void GotDynamicData(List<RigDynamicData> rigsDynamicData)
    {
        _rigsDynamicDataCounter.UpdateData(rigsDynamicData);
        _rigsDynamicDataStream.OnNext(rigsDynamicData);
    }

    /// <inheritdoc/>
    public async Task GotRigsState(string subscriberId, List<RigState> rigs)
    {
        var mediator = GetMediator();

        await NotifyAboutGettingRigsState(mediator, subscriberId, rigs);

        foreach (var rigState in rigs)
        {
            await _rigsStateCache.SetAsync(rigState.Id.ToString(), rigState);
        }
    }

    /// <summary>
    /// Уведомить подписчика о получении состояния ригов.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="rigsStates"> Состояние ригов. </param>
    private async Task NotifyAboutGettingRigsState(IMediator mediator, string subscriberId, List<RigState> rigsStates)
    {
        if (_subscriberSpecifications.TryGetValue(subscriberId, out var subscriberSpecification))
        {
            await mediator.Publish(new RigsStateReceivedEvent(subscriberId,
                                                              new Specification(_userId,
                                                                                subscriberSpecification.RigsSearchString,
                                                                                subscriberSpecification.FilterString,
                                                                                subscriberSpecification.FilterArguments),
                                                              rigsStates));
        }
    }

    /// <summary>
    /// Уведомить об изменении динамических данных ригов.
    /// </summary>
    private async Task NotifyRigsDynamicDataUpdated()
    {
        var scope = _serviceScopeFactory.CreateScope();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        // отправить событие об изменении обобщённых данных.
        await mediator.Publish(
            new UpdateTotalDynamicDataEvent(_userId,
                                            new TotalDynamicData(_rigsDynamicDataCounter.GetTotalPower(),
                                                                 _rigsDynamicDataCounter.GetTotalShares(),
                                                                 _rigsDynamicDataCounter.GetTotalCoinStatistics())));

        // отправить события об изменении динамических данных в соответствии со спецификацией каждого подписчика.
        foreach (var subscriber in _rigsDynamicDataStreamSubscriptions.Keys)
        {
            var specification = _subscriberSpecifications.GetValueOrDefault(subscriber);
            var rigsDynamicData = await _rigsDynamicDataCounter.GetRigsDynamicData(mediator, specification);

            HashRateModel? hashRate = null;

            if (!string.IsNullOrEmpty(specification.ObservableCoin))
            {
                hashRate = new HashRateModel()
                {
                    Time = DateTimeOffset.Now,
                    Value = mapper.Map<ParameterModelWithMeasureUnit>(
                                _rigsDynamicDataCounter.GetTotalHashRate(specification))
                };
            }

            await mediator.Publish(new SubscriberRigsDynamicDataUpdateEvent(
                                        subscriber,
                                        new Specification(_userId,
                                                          specification.RigsSearchString,
                                                          specification.FilterString,
                                                          specification.FilterArguments),
                                        rigsDynamicData,
                                        hashRate));
        }
    }

    /// <summary>
    /// Отправить подписчику состояние ригов.
    /// </summary>
    /// <param name="mediator"> Медиатор. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    private async Task SendRigsState(IMediator mediator, string subscriberId)
    {
        var rigsState = await GetOrRequestRigsState(mediator, subscriberId);

        if (rigsState.Count != 0)
        {
            await NotifyAboutGettingRigsState(mediator, subscriberId, rigsState);
        }
    }

    /// <summary>
    /// Получить или запросить состояние ригов.
    /// </summary>
    /// <param name="mediator"> Медиатор. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <returns> Состояние ригов, которое удалось достать из кэша. </returns>
    private async Task<List<RigState>> GetOrRequestRigsState(IMediator mediator, string subscriberId)
    {
        var rigsIds = await mediator.Send(new GetRigsIdsQuery(new Specification(_userId)));

        var states = new List<RigState>();

        foreach (var rigId in rigsIds)
        {
            var state = await _rigsStateCache.GetOrDefaultAsync<RigState>(rigId.ToString());

            if (state is not null)
            {
                states.Add(state);
            }
            else
            {
                // если хотя бы одного рига нет в кэше, то запрашиваем состояние всех.
                // todo: надо проработать момент, когда один подписчик запросил данные, но они ещё не успели прийти, и запрашивает второй подписчик.
                await mediator.Publish(new RigsStateWaitingEvent(_userId, subscriberId));
                break;
            }
        }

        return states;
    }

    /// <summary>
    /// Подписаться на поток динамических данных с ригов.
    /// </summary>
    /// <param name="mediator"> Медиатор. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    private async Task SubscribeToDynamicDataStream(IMediator mediator, string subscriberId)
    {
        var subscription = _rigsDynamicDataStream
                .Sample(TimeSpan.FromSeconds(_dynamicDataOptions.UpdatePeriodInSeconds))
                .Subscribe(async data => await NotifyRigsDynamicDataUpdated());

        if (_rigsDynamicDataStreamSubscriptions.TryAdd(subscriberId, subscription))
        {
            if (_rigsDynamicDataStreamSubscriptions.Count == 1)
            {
                await mediator.Publish(new DynamicDataWaitingEvent(_userId));
            }
        }
    }

    /// <summary>
    /// Получить экземпляр посредника.
    /// </summary>
    /// <returns> Посредник. </returns>
    private IMediator GetMediator()
    {
        var scope = _serviceScopeFactory.CreateScope();
        return scope.ServiceProvider.GetRequiredService<IMediator>();
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
                foreach (var subscriber in _subscriberSpecifications)
                {
                    _subscriberSpecifications.TryRemove(subscriber.Key, out var _);
                }

                foreach (var subscriberSubscription in _rigsDynamicDataStreamSubscriptions)
                {
                    if (_rigsDynamicDataStreamSubscriptions.TryRemove(subscriberSubscription.Key, out var subscription))
                    {
                        subscription.Dispose();
                    }
                }

                _rigsDynamicDataCounter.Dispose();
            }

            _subscriberSpecifications = null!;
            _rigsDynamicDataCounter = null!;
            _dynamicDataOptions = null!;
            _disposedValue = true;
            _rigsDynamicDataStream = null!;
            _rigsDynamicDataStreamSubscriptions = null!;
        }
    }
}
