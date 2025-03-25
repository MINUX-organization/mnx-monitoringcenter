using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Streams;

public class UnionStreamBuilder : IUnionStreamBuilder
{
    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;

    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UnionStreams.Abstractions.Stream Build(UnionStreamBuilderArgs streamBuilderArgs)
    {
        return streamBuilderArgs.StreamType switch
        {
            StreamType.Monitoring => new MonitoringStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                _userRigsObserverAggregator,
                _serviceScopeFactory),

            StreamType.Devices => new DevicesStream(
                streamBuilderArgs.UserId, 
                streamBuilderArgs.ConnectionId, 
                _userRigsObserverAggregator,
                _serviceScopeFactory),

            StreamType.Rigs => new RigsStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                _userRigsObserverAggregator,
                _serviceScopeFactory),

            _ => throw new NotImplementedException(),
        };
    }

    public UnionStreamBuilder(IUserRigsObserverAggregator userRigsObserverAggregator,
        IServiceScopeFactory serviceScopeFactory)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _userRigsObserverAggregator = userRigsObserverAggregator;
    }
}