using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;
using MNX.MonitoringCenter.Traffic.Observers;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Streams;

public class UnionStreamBuilder : IUnionStreamBuilder
{
    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    private readonly IOptionsMonitor<DynamicIndicatorsOptions> _optionsMonitor;

    public UnionStreams.Abstractions.Stream Build(UnionStreamBuilderArgs streamBuilderArgs)
    {
        return streamBuilderArgs.StreamType switch
        {
            StreamType.Monitoring => new MonitoringStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                _userRigsObserverAggregator,
                _serviceScopeFactory,
                _optionsMonitor),

            StreamType.Devices => new DevicesStream(
                streamBuilderArgs.UserId, 
                streamBuilderArgs.ConnectionId, 
                _userRigsObserverAggregator,
                _serviceScopeFactory,
                _optionsMonitor),

            StreamType.Rigs => new RigsStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                _userRigsObserverAggregator,
                _serviceScopeFactory,
                _optionsMonitor),

            _ => throw new NotImplementedException(),
        };
    }

    public UnionStreamBuilder(IUserRigsObserverAggregator userRigsObserverAggregator,
                              IServiceScopeFactory serviceScopeFactory,
                              IOptionsMonitor<DynamicIndicatorsOptions> optionsMonitor)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _userRigsObserverAggregator = userRigsObserverAggregator;

        _optionsMonitor = optionsMonitor ?? throw new ArgumentNullException(nameof(optionsMonitor));
    }
}