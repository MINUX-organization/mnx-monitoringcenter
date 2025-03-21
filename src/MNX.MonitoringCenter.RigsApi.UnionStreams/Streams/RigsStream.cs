using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers;
using System.Reactive.Linq;
using System.Threading.Channels;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.Application.UseCases.Mediator;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class RigsStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    protected override SubscriptionType[] SubscriptionTypes => new[] {
        SubscriptionType.GeneralHardwareRigsIndicators,
    };

    public RigsStream(
        Guid userId,
        string connectionId,
        IUserRigsObserverAggregator userRigsObserverAggregator,
        IServiceScopeFactory scope)
    {
        _serviceScopeFactory = scope;

        _userRigsObserverAggregator = userRigsObserverAggregator;

        _subscription = Subject
            .Buffer(TimeSpan, SubscriptionTypes.Length)
            .Select(item => OnNewDataReceived(item, userId))
            .Concat()
            .Subscribe(
                response => _channel.Writer.TryWrite(response),
                ex => _channel.Writer.TryComplete(ex),
                () => _channel.Writer.TryComplete()
            );

        foreach (var subscriptionType in SubscriptionTypes)
        {
            var isSubscribed = userRigsObserverAggregator.TrySubscribe(userId, connectionId, subscriptionType,
                data => Subject.OnNext((subscriptionType, data)));
        }
    }

    private async Task<IEnumerable<RigDynamicHardwareIndicatorsModel>> OnNewDataReceived(
        IList<(SubscriptionType, object)> data, Guid userId)
    {
        using var scope = _serviceScopeFactory!.CreateScope();
        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        var messages = data
            .GroupBy(x => x.Item1)
            .Select(x => x.Last())
            .ToDictionary(x => x.Item1, x => x.Item2);

        var response = RigDynamicHardwareIndicatorsModel.ConvertFrom(new RigDynamicHardwareIndicatorsModelArgs(
            messages.GetValueOrDefault(SubscriptionType.GeneralHardwareRigsIndicators)
                as IEnumerable<RigDynamicHardwareIndicators>,
            (await mediator.GetListAsync(new GetRigsQuery(userId), default)).ToDictionary(x => x.Id, x => x)));

        if (response is null)
        {
            return new List<RigDynamicHardwareIndicatorsModel>();
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
