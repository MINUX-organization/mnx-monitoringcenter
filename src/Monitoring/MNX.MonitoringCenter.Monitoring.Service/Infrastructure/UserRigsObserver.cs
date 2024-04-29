using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateDynamicData;
using MNX.MonitoringCenter.Monitoring.UseCases.Notifications.UpdateTotalDynamicData;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

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
    private readonly long _userId;

    /// <summary>
    /// Поток динамических данных с ригов.
    /// </summary>
    private ISubject<List<RigDynamicData>> _rigsDynamicDataStream;

    /// <summary>
    /// Подписка на поток динамических данных с ригов.
    /// </summary>
    private IDisposable _rigsDynamicDataStreamSubscription;

    /// <summary>
    /// Спецификации фильтрации данных для каждого подписчика.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор подписчика.
    /// Значение - спецификация.
    /// </remarks>
    private ConcurrentDictionary<string, RigsDynamicDataSpecification> _subscriberSpecifications = new();

    /// <summary>
    /// Счётчик динамических данных ригов.
    /// </summary>
    private UserRigsDynamicDataCounter _rigsDynamicDataCounter;

    public UserRigsObserver(IServiceScopeFactory serviceScopeFactory,
                            long userId,
                            DynamicDataOptions dynamicDataOptions)
    {
        _rigsDynamicDataCounter = new UserRigsDynamicDataCounter(userId, dynamicDataOptions);

        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _userId = userId;

        _rigsDynamicDataStream = new Subject<List<RigDynamicData>>();
        _rigsDynamicDataStreamSubscription = _rigsDynamicDataStream
                    .Sample(TimeSpan.FromSeconds(dynamicDataOptions.UpdatePeriodInSeconds))
                    .Subscribe(async x => await NotifyRigsDynamicDataUpdated());
    }

    /// <inheritdoc/>
    public async Task<long> Subscribe(string subscriberId)
    {
        var mediator = GetMediator();

        var subscribersCount = Interlocked.Increment(ref _subscribersCount);

        await mediator.Publish(new ClientSubscriptionEvent(_userId, subscriberId, subscribersCount));

        var result = await mediator.Send(new GetRigsInformationQuery(new Specification(_userId)));
        await mediator.Publish(new GotRigsInformationEvent(subscriberId, result));

        return subscribersCount;
    }

    /// <inheritdoc/>
    public async Task<long> Unsubscribe(string subscriberId)
    {
        _subscriberSpecifications.TryRemove(subscriberId, out var _);

        var subscribersCount = Interlocked.Decrement(ref _subscribersCount);

        if (subscribersCount <= 0)
        {
            var mediator = GetMediator();

            await mediator.Publish(new LastClientUnsubscribedEvent(_userId));
        }

        return subscribersCount;
    }

    /// <inheritdoc/>
    public async Task SetObservableCoin(string subscriberId, string coin)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();

        _subscriberSpecifications.AddOrUpdate(subscriberId,
                                    new RigsDynamicDataSpecification() { ObservableCoin = coin },
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
    public void GotDynamicData(List<RigDynamicData> rigDynamicData)
    {
        _rigsDynamicDataCounter.UpdateData(rigDynamicData);
        _rigsDynamicDataStream.OnNext(rigDynamicData);
    }

    /// <inheritdoc/>
    public async Task GotRigsState(string subscriberId, List<RigState> rigs)
    {
        if (_subscriberSpecifications.TryGetValue(subscriberId, out var subscriber))
        {
            var mediator = GetMediator();

            await mediator.Publish(new RigsStateReceivedEvent(subscriberId,
                                                              new Specification(_userId,
                                                                                subscriber.RigsSearchString,
                                                                                subscriber.FilterString,
                                                                                subscriber.FilterArguments),
                                                              rigs));
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

        await mediator.Publish(
            new UpdateTotalDynamicDataEvent(_userId,
                                            new TotalDynamicData(_rigsDynamicDataCounter.GetTotalPower(),
                                                                 _rigsDynamicDataCounter.GetTotalShares(),
                                                                 _rigsDynamicDataCounter.GetTotalCoinStatistics())));

        foreach (var subscriberSpecification in _subscriberSpecifications)
        {
            var rigsDynamicData = await _rigsDynamicDataCounter.GetRigsDynamicData(mediator, subscriberSpecification.Value);

            HashRateModel? hashRate = null;

            if (!string.IsNullOrEmpty(subscriberSpecification.Value.ObservableCoin))
            {
                hashRate = new HashRateModel()
                {
                    Time = DateTimeOffset.Now,
                    Value = mapper.Map<ParameterModelWithMeasureUnit>(
                                _rigsDynamicDataCounter.GetTotalHashRate(subscriberSpecification.Value))
                };
            }

            await mediator.Publish(new SubscriberRigsDynamicDataUpdateEvent(
                                        subscriberSpecification.Key,
                                        new Specification(_userId,
                                                          subscriberSpecification.Value.RigsSearchString,
                                                          subscriberSpecification.Value.FilterString,
                                                          subscriberSpecification.Value.FilterArguments),
                                        rigsDynamicData,
                                        hashRate));
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

                _rigsDynamicDataCounter.Dispose();
                _rigsDynamicDataStreamSubscription.Dispose();
            }

            _subscriberSpecifications = null!;
            _rigsDynamicDataCounter = null!;
            _disposedValue = true;
            _rigsDynamicDataStream = null!;
            _rigsDynamicDataStreamSubscription = null!;
        }
    }
}
