using AutoMapper;
using EasyNetQ;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using MNX.MonitoringCenter.Monitoring.Contracts.Abstractions;
using MNX.MonitoringCenter.Monitoring.Contracts.Enums;
using MNX.MonitoringCenter.Monitoring.Contracts.Messages.Bus;
using MNX.MonitoringCenter.Monitoring.Contracts.Models;
using MNX.MonitoringCenter.Monitoring.Hubs;
using MNX.MonitoringCenter.Monitoring.Service.Hubs.Clients;
using MNX.MonitoringCenter.Monitoring.Service.Messages;
using MNX.MonitoringCenter.Monitoring.Service.Messages.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Abstractions;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries;
using MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsIds;
using System.Collections.Concurrent;

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
    /// Отправитель сообщений в шину.
    /// </summary>
    private readonly IPubSub _pubSub;

    /// <summary>
    /// Количество подписчиков.
    /// </summary>
    private long _subscribersCount;

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    private readonly long _userId;

    /// <summary>
    /// Спецификации фильтрации данных для каждого подписчика.
    /// </summary>
    /// <remarks>
    /// Ключ - идентификатор подписчика.
    /// Значение - спецификация.
    /// </remarks>
    private ConcurrentDictionary<string, RigsDynamicDataSpecification> _subscribers = new();

    /// <summary>
    /// Счётчик динамических данных ригов.
    /// </summary>
    private UserRigsDynamicDataCounter _rigsDynamicDataCounter;

    public UserRigsObserver(IServiceScopeFactory serviceScopeFactory,
                            IPubSub pubSub,
                            long userId)
    {
        _rigsDynamicDataCounter = new UserRigsDynamicDataCounter(userId);

        _serviceScopeFactory = serviceScopeFactory
            ?? throw new ArgumentNullException(nameof(serviceScopeFactory));

        _pubSub = pubSub ?? throw new ArgumentNullException(nameof(pubSub));

        _userId = userId;
    }

    /// <inheritdoc/>
    public async Task<long> Subscribe(string subscriberId)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MonitoringHub, IMonitoringClient>>();

        var subscribersCount = Interlocked.Increment(ref _subscribersCount);

        var result = await mediator.Send(new ClientSubscriptionMessage(subscribersCount, new SubscriptionModel()
        {
            UserId = _userId,
            ConnectionId = subscriberId,
        }));

        await SendRigsInformation(result, subscriberId, hubContext);

        return subscribersCount;
    }

    /// <inheritdoc/>
    public async Task<long> Unsubscribe(string subscriberId)
    {
        _subscribers.TryRemove(subscriberId, out var _);

        var subscribersCount = Interlocked.Decrement(ref _subscribersCount);

        if (subscribersCount <= 0)
        {
            await StopStatisticsStream();
        }

        return subscribersCount;
    }

    /// <inheritdoc/>
    public void SetObservableCoin(string subscriberId, string coin)
    {
        _subscribers.AddOrUpdate(subscriberId,
                                    new RigsDynamicDataSpecification() { ObservableCoin = coin },
                                    (_, value) =>
                                    {
                                        value.ObservableCoin = coin;
                                        return value;
                                    });
    }

    /// <inheritdoc/>
    public async Task GotDynamicData(List<RigDynamicData> rigDynamicData)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MonitoringHub, IMonitoringClient>>();
        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        _rigsDynamicDataCounter.UpdateData(rigDynamicData);

        await SendRigsDynamicData(hubContext, mapper, mediator);
        await SendTotalData(hubContext);
    }

    /// <inheritdoc/>
    public async Task GotRigsState(string subscriberId, List<RigState> rigs)
    {
        var scope = _serviceScopeFactory.CreateScope();
        var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<MonitoringHub, IMonitoringClient>>();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        if (_subscribers.TryGetValue(subscriberId, out var subscriber))
        {
            var rigsIds = await mediator.Send(new GetRigsIdsQuery(new Specification(_userId,
                                                                                    subscriber.RigsSearchString,
                                                                                    subscriber.FilterString,
                                                                                    subscriber.FilterArguments)));

            var response = (IEnumerable<RigState?>)MapToResponse(rigsIds.ToList(), rigs.ToArray());

            await hubContext.Clients.Client(subscriberId).ReceivedRigsState(response);
        }
    }

    /// <summary>
    /// Подготавливает данные к ответу.
    /// </summary>
    /// <param name="rigsIds"></param>
    /// <param name="rigs"></param>
    /// <returns></returns>
    private static List<IRig?> MapToResponse(List<Guid> rigsIds, IEnumerable<IRig> rigs)
    {
        List<IRig?> response = new();

        for (int i = 0; i < rigsIds.Count; i++)
        {
            var rig = rigs.FirstOrDefault(x => x.Id == rigsIds[i]);

            if (rig is not null)
            {
                response.Add(rig);
                continue;
            }

            response.Add(null);
        }

        return response;
    }

    /// <summary>
    /// Отправить динамические данные подписчикам.
    /// </summary>
    private async Task SendRigsDynamicData(IHubContext<MonitoringHub, IMonitoringClient> hubContext,
                                           IMapper mapper,
                                           IMediator mediator)
    {
        foreach (var subscriber in _subscribers)
        {
            var data = await _rigsDynamicDataCounter.GetRigsDynamicData(mediator, subscriber.Value);

            var rigsIds = await mediator.Send(new GetRigsIdsQuery(new Specification(_userId,
                                                                            subscriber.Value.RigsSearchString,
                                                                            subscriber.Value.FilterString,
                                                                            subscriber.Value.FilterArguments)));

            var response = (IEnumerable<RigDynamicData?>)MapToResponse(rigsIds.ToList(), data.ToArray());

            await hubContext.Clients.Client(subscriber.Key)
                .ReceivedRigsDynamicData(response);

            await SendHashRate(subscriber.Key, subscriber.Value, hubContext, mapper);
        }
    }

    /// <summary>
    /// Отправить общие данные подписчикам.
    /// </summary>
    private async Task SendTotalData(IHubContext<MonitoringHub, IMonitoringClient> hubContext)
    {
        await hubContext.Clients.User(_userId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalPower,
                NewData = _rigsDynamicDataCounter.GetTotalPower()
            });

        await hubContext.Clients.User(_userId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalShares,
                NewData = _rigsDynamicDataCounter.GetTotalShares()
            });

        await hubContext.Clients.User(_userId.ToString())
            .ReceivedTotalData(new TotalDataChangeMessage()
            {
                Type = TotalDataType.TotalCoinsList,
                NewData = _rigsDynamicDataCounter.GetTotalCoinStatistics()
            });
    }

    /// <summary>
    /// Отправить скорость хеширования подписчикам.
    /// </summary>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    /// <param name="specification"> Спецификация. </param>
    private async Task SendHashRate(string subscriberId,
                                    RigsDynamicDataSpecification specification,
                                    IHubContext<MonitoringHub, IMonitoringClient> hubContext,
                                    IMapper mapper)
    {
        if (string.IsNullOrEmpty(specification.ObservableCoin))
        {
            return;
        }

        await hubContext.Clients.Client(subscriberId)
            .ReceivedCurrentHashRate(new HashRateModel()
            {
                Time = DateTimeOffset.Now,
                Value = mapper.Map<ParameterModelWithMeasureUnit>(
                    _rigsDynamicDataCounter.GetTotalHashRate(specification))
            });

        // todo: ! 5 мин кэш
    }

    /// <summary>
    /// Отправить информацию о ригах подписчикам.
    /// </summary>
    /// <param name="result"> Результат подписки клиента. </param>
    /// <param name="subscriberId"> Идентификатор подписчика. </param>
    private static async Task SendRigsInformation(SubscribeClientResult result,
                                                  string subscriberId,
                                                  IHubContext<MonitoringHub, IMonitoringClient> hubContext)
    {
        await hubContext.Clients.Client(subscriberId).ReceivedRigsInformation(result.Rigs);

        await hubContext.Clients.Client(subscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalRigsCount,
            NewData = result.TotalRigsCount
        });

        await hubContext.Clients.Client(subscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalGpusCount,
            NewData = result.TotalGpusCount
        });

        await hubContext.Clients.Client(subscriberId).ReceivedTotalData(new TotalDataChangeMessage()
        {
            Type = TotalDataType.TotalCpusCount,
            NewData = result.TotalCpusCount
        });
    }

    /// <summary>
    /// Остановить поток статистики.
    /// </summary>
    private async Task StopStatisticsStream()
    {
        await _pubSub.PublishAsync(new StatisticsStreamStopCommand
        {
            UserId = _userId,
            ObservableObjectsType = ObservableObjectsType.Rigs
        }, cancellationToken: default);
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
                foreach (var subscriber in _subscribers)
                {
                    _subscribers.TryRemove(subscriber.Key, out var _);
                }

                _rigsDynamicDataCounter.Dispose();
            }

            _subscribers = null!;
            _rigsDynamicDataCounter = null!;
            _disposedValue = true;
        }
    }
}
