using MediatR;
using System.Reactive.Linq;
using System.Threading.Channels;
using Microsoft.Extensions.Options;
using MNX.Application.UseCases.Mediator;
using MNX.MonitoringCenter.Traffic.Observers;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Streams;
using MNX.MonitoringCenter.Management.UseCases.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class DevicesStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    protected override SubscriptionType[] SubscriptionTypes => new[] {
        SubscriptionType.CpusHardwareIndicators,
        SubscriptionType.GpusHardwareIndicators,
        SubscriptionType.CpusMiningIndicators,
        SubscriptionType.GpusMiningIndicators,
    };

    public DevicesStream(
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

    private async Task<DevicesIndicatorsStreamResponse> OnNewDataReceived(
        IList<(SubscriptionType, object)> data,
        Guid userId)
    {
        using var scope = _serviceScopeFactory!.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = data
            .GroupBy(x => x.Item1)
            .Select(x => x.Last())
            .ToDictionary(x => x.Item1, x => x.Item2);

        var cpuNames = new Dictionary<Guid, string>();
        var gpusNames = new Dictionary<Guid, string>();
        var miningDevices = await mediator.GetListAsync(new GetAvailableMiningDevicesQuery(userId), default);

        foreach (var miningDevice in miningDevices)
        {
            if (miningDevice.Type == "CPU")
            {
                cpuNames.Add(miningDevice.Id, miningDevice.Name);
            }

            if (miningDevice.Type == "GPU")
            {
                gpusNames.Add(miningDevice.Id, miningDevice.Name);
            }
        }

        var response = DevicesIndicatorsStreamResponse.ConvertFrom(new DevicesIndicatorsStreamResponseArgs(
            messages.GetValueOrDefault(SubscriptionType.CpusMiningIndicators) 
                as IEnumerable<CpuDynamicMiningIndicators>,
            messages.GetValueOrDefault(SubscriptionType.CpusHardwareIndicators) 
                as IEnumerable<CpuDynamicHardwareIndicators>,
            messages.GetValueOrDefault(SubscriptionType.GpusMiningIndicators) 
                as IEnumerable<GpuDynamicMiningIndicators>,
            messages.GetValueOrDefault(SubscriptionType.GpusHardwareIndicators) 
                as IEnumerable<GpuDynamicHardwareIndicators>,
            cpuNames,
            gpusNames,
            await mediator.Send(new GetMiningCombinationsQuery(userId), default)));

        if (response is null)
        {
            return new DevicesIndicatorsStreamResponse();
        }

        return response;
    }

    public override async IAsyncEnumerable<object> StartStreaming()
    {
        await foreach (var response in _channel.Reader.ReadAllAsync())
        {
            yield return response;
        }
    }

    public override void StopStreaming(Guid userId, string connectionId)
    {
        _subscription.Dispose();
        _channel.Writer.TryComplete();
        _userRigsObserverAggregator.UnsubscribeFromAll(userId, connectionId);
    }
}