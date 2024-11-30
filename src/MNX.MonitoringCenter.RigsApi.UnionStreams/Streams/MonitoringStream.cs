using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Streams.Monitoring;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts;
using System.Reactive.Linq;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams
{
    public class MonitoringStream : Abstractions.Stream
    {
        private readonly IDisposable _subscription;

        private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

        private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

        private readonly Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> _miningCombinations;

        protected override SubscriptionType[] SubscriptionTypes => new[] {
            SubscriptionType.TotalCoinsStatistics,
            SubscriptionType.TotalShares,
            SubscriptionType.TotalPower,
            SubscriptionType.TotalHashRate,
            SubscriptionType.GeneralMiningRigsIndicators,
        };

        public MonitoringStream(
            Guid userId,
            Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombinations> miningCombinations,
            string connectionId,
            IUserRigsObserverAggregator userRigsObserverAggregator)
        {
            _userRigsObserverAggregator = userRigsObserverAggregator;

            _miningCombinations = miningCombinations;

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
                    data => { Subject.OnNext((subscriptionType, data)); Console.WriteLine(data); });
            }
        }

        private MonitoringIndicatorsResponse OnNewDataReceived(IList<(SubscriptionType, object)> data)
        {
            var messages = data.ToDictionary(x => x.Item1, x => x.Item2);

            var response = MonitoringIndicatorsResponse.ConvertFrom(new MonitoringIndicatorsResponseArgs(
                (IEnumerable<CoinStatistics>)messages[SubscriptionType.TotalCoinsStatistics],
                (SharesModel)messages[SubscriptionType.TotalShares],
                (int)messages[SubscriptionType.TotalHashRate],
                (int)messages[SubscriptionType.TotalPower],
                (IEnumerable<RigDynamicMiningIndicators>)messages[SubscriptionType.GeneralMiningRigsIndicators],
                _miningCombinations
            ));

            if (response is null)
            {
                return new MonitoringIndicatorsResponse();
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
}
