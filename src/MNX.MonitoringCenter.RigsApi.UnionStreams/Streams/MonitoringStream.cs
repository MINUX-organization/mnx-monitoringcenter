using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Streams;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using System.Reactive.Linq;
using System.Threading.Channels;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class MonitoringStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    private readonly Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> _miningCombinations;

    private readonly Dictionary<Guid, RigDetails> _rigs;

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
        Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> miningCombinations,
        IEnumerable<RigDetails> rigs,
        IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _userRigsObserverAggregator = userRigsObserverAggregator;
        _miningCombinations = miningCombinations;
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

    private MonitoringIndicatorsStreamResponse OnNewDataReceived(IList<(SubscriptionType, object)> data)
    {
        var messages = data.ToDictionary(x => x.Item1, x => x.Item2);

        var response = MonitoringIndicatorsStreamResponse.ConvertFrom(new MonitoringIndicatorsStreamResponseArgs(
            (IEnumerable<CoinStatistics>)messages[SubscriptionType.TotalCoinsStatistics],
            (SharesModel)messages[SubscriptionType.TotalShares],
            (int)messages[SubscriptionType.TotalHashRate],
            (int)messages[SubscriptionType.TotalPower],
            (IEnumerable<RigDynamicMiningIndicators>)messages[SubscriptionType.GeneralMiningRigsIndicators],
            (IEnumerable<RigDynamicHardwareIndicators>)messages[SubscriptionType.GeneralHardwareRigsIndicators],
            _miningCombinations,
            _rigs
        ));

        if (response is null)
        {
            // TODO: Реализация отправки прошлого результата.
            return new MonitoringIndicatorsStreamResponse();
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
