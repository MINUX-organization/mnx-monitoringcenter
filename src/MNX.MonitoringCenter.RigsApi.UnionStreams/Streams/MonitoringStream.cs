using MediatR;
using System.Reactive.Linq;
using System.Threading.Channels;
using MNX.Application.UseCases.Mediator;
using MNX.MonitoringCenter.Traffic.Observers;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Streams;
using MNX.MonitoringCenter.Management.UseCases.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class MonitoringStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    protected override SubscriptionType[] SubscriptionTypes => new[] {
        SubscriptionType.TotalCoinsStatistics,
        SubscriptionType.TotalShares,
        SubscriptionType.TotalPower,
        SubscriptionType.TotalHashRate,
        SubscriptionType.GeneralMiningRigsIndicators,
        SubscriptionType.GeneralHardwareRigsIndicators,
    };

    public MonitoringStream(
        Guid userId,
        string connectionId,
        IUserRigsObserverAggregator userRigsObserverAggregator,
        IServiceScopeFactory scope,
        IOptionsMonitor<DynamicIndicatorsOptions> optionsMonitor)
    {
        _serviceScopeFactory = scope;
        _userRigsObserverAggregator = userRigsObserverAggregator;

        var configChanges = Observable.Create<DynamicIndicatorsOptions>(observer =>
        {
            observer.OnNext(optionsMonitor.CurrentValue);
            return optionsMonitor.OnChange(config => observer.OnNext(config));
        });

        var dynamicBufferedStream = configChanges
            .Select(config =>
            {
                var period = TimeSpan.FromSeconds(config.TimeOut);
                return Observable.Interval(period).StartWith(0);
            })
            .Switch()
            .Publish(trigger =>
                Subject.Window(trigger)
                    .SelectMany(window =>
                    {
                        return window.Take(SubscriptionTypes.Length).ToList();
                    })
                    .Select(list => OnNewDataReceived(list, userId))
                    .Concat()
            );

        _subscription = dynamicBufferedStream.Subscribe(
            response => _channel.Writer.TryWrite(response),
            ex => _channel.Writer.TryComplete(ex),
            () => _channel.Writer.TryComplete());

        foreach (var subscriptionType in SubscriptionTypes)
        {
            var isSubscribed = userRigsObserverAggregator.TrySubscribe(userId, connectionId, subscriptionType,
                data => Subject.OnNext((subscriptionType, data)));
        }
    }

    /// <summary>
    /// Обработка новых данных.
    /// </summary>
    /// <param name="data"> Данные. </param>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <returns> Задача с результатом ответа. </returns>
    private async Task<MonitoringIndicatorsStreamResponse> OnNewDataReceived(
        IList<(SubscriptionType, object)> data,
        Guid userId)
    {   
        using var scope = _serviceScopeFactory!.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = data
            .GroupBy(x => x.Item1)
            .Select(x => x.Last())
            .ToDictionary(x => x.Item1, x => x.Item2);

        var response = MonitoringIndicatorsStreamResponse.ConvertFrom(new MonitoringIndicatorsStreamResponseArgs(
            messages.GetValueOrDefault(SubscriptionType.TotalCoinsStatistics) as IEnumerable<CoinStatistics>,
            messages.GetValueOrDefault(SubscriptionType.TotalShares) as SharesModel,
            int.Parse(messages.GetValueOrDefault(SubscriptionType.TotalHashRate)?.ToString() ?? "0"),
            int.Parse(messages.GetValueOrDefault(SubscriptionType.TotalPower)?.ToString() ?? "0"),
            messages.GetValueOrDefault(SubscriptionType.GeneralMiningRigsIndicators) 
                as IEnumerable<RigDynamicMiningIndicators>,
            messages.GetValueOrDefault(SubscriptionType.GeneralHardwareRigsIndicators) 
                as IEnumerable<RigDynamicHardwareIndicators>,
            await mediator.Send(new GetMiningCombinationsQuery(userId), default),
            (await mediator.GetListAsync(
                new GetRigsDetailsQuery(userId), default))
                .ToDictionary(x => x.Id, x => x)
        ));

        if (response is null)
        {
            return new MonitoringIndicatorsStreamResponse();
        }

        return response;
    }

    /// <inheritdoc/>
    public override async IAsyncEnumerable<object> StartStreaming()
    {
        await foreach (var response in _channel.Reader.ReadAllAsync())
        {
            yield return response;
        }
    }

    /// <inheritdoc/>
    public override void StopStreaming(Guid userId, string connectionId)
    {
        _subscription.Dispose();
        _channel.Writer.TryComplete();
        _userRigsObserverAggregator.UnsubscribeFromAll(userId, connectionId);
    }
}
