using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.RigsApi.Contracts.Args;
using MNX.MonitoringCenter.RigsApi.Contracts.Streams;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;
using System.Reactive.Linq;
using System.Threading.Channels;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;

public class DevicesStream : Abstractions.Stream
{
    private readonly IDisposable _subscription;

    private readonly Channel<object> _channel = Channel.CreateUnbounded<object>();

    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    private readonly IAsyncEnumerable<CpuDetails> _cpuDetails;

    private readonly IAsyncEnumerable<GpuDetails> _gpusDatails;

    private readonly Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination> _miningCombinations;

    protected override SubscriptionType[] SubscriptionTypes => new[] {
        SubscriptionType.CpusHardwareIndicators,
        SubscriptionType.GpusHardwareIndicators,
        SubscriptionType.CpusMiningIndicators,
        SubscriptionType.GpusMiningIndicators,
    };

    public DevicesStream(
        Guid userId,
        string connectionId,
        IAsyncEnumerable<CpuDetails> cpusNames,
        IAsyncEnumerable<GpuDetails>  gpusNames,
        Dictionary<(Guid FlightSheetId, Guid MinerId, Guid CoinId), MiningCombination> miningCombinations,
        IUserRigsObserverAggregator userRigsObserverAggregator)
    {
        _userRigsObserverAggregator = userRigsObserverAggregator;
        _cpuDetails = cpusNames;
        _gpusDatails = gpusNames;
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
                data => Subject.OnNext((subscriptionType, data)));
        }
    }

    private async Task<DevicesIndicatorsStreamResponse> OnNewDataReceived(IList<(SubscriptionType, object)> data)
    {
        var messages = data.ToDictionary(x => x.Item1, x => x.Item2);

        var cpusNames = new Dictionary<Guid, string>();
        var gpusNames = new Dictionary<Guid, string>();

        await foreach (var cpuDetails in _cpuDetails)
        {
            cpusNames.Add(cpuDetails.Id, cpuDetails.Information.Name);
        }

        await foreach (var gpuDetails in _gpusDatails)
        {
            gpusNames.Add(gpuDetails.Id, gpuDetails.Information.Name);
        }

        var response = DevicesIndicatorsStreamResponse.ConvertFrom(new DevicesIndicatorsStreamResponseArgs(
            (IEnumerable<CpuDynamicMiningIndicators>)messages[SubscriptionType.CpusMiningIndicators],
            (IEnumerable<CpuDynamicHardwareIndicators>)messages[SubscriptionType.CpusHardwareIndicators],
            (IEnumerable<GpuDynamicMiningIndicators>)messages[SubscriptionType.GpusMiningIndicators],
            (IEnumerable<GpuDynamicHardwareIndicators>)messages[SubscriptionType.GpusHardwareIndicators],
            cpusNames,
            gpusNames,
            _miningCombinations));

        if (response is null)
        {
            // TODO: Реализация отправки прошлого результата.
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