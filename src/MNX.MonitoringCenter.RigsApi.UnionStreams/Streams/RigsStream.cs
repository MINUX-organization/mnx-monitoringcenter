using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers;
using System.Reactive.Linq;
using System.Threading.Channels;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class RigsStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    private readonly Dictionary<Guid, Rig> _rigs;

    protected override SubscriptionType[] SubscriptionTypes => new[] {
        SubscriptionType.GeneralHardwareRigsIndicators,
    };

    public RigsStream(
        Guid userId,
        string connectionId,
        List<Rig> rigs,
        IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _userRigsObserverAggregator = userRigsObserverAggregator;

        _rigs = rigs.ToDictionary(x => x.Id);

        _subscription = Subject
            .GroupBy(x => x.Item1)
            .SelectMany(group => group.Buffer(SubscriptionTypes.Length))
            .Select(OnNewDataReceived)
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

    private IEnumerable<RigDynamicHardwareIndicatorsModel> OnNewDataReceived(IList<(SubscriptionType, object)> data)
    {
        var messages = data.ToDictionary(x => x.Item1, x => x.Item2);

        var response = RigDynamicHardwareIndicatorsModel.ConvertFrom(new RigDynamicHardwareIndicatorsModelArgs(
            (IEnumerable<RigDynamicHardwareIndicators>)messages[SubscriptionType.GeneralHardwareRigsIndicators],
            _rigs));

        if (response is null)
        {
            // TODO: Реализация отправки прошлого результата.
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
