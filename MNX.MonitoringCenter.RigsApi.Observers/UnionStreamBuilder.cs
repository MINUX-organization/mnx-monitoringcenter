using MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Combinations.Queries;
using MNX.MonitoringCenter.RigsApi.Args;

namespace MNX.MonitoringCenter.RigsApi.Streams;

public class UnionStreamBuilder : IUnionStreamBuilder
{
    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;
    private readonly IMediator _mediator;

    public async Task<UnionStreams.Abstractions.Stream> Build(UnionStreamBuilderArgs streamBuilderArgs)
    {
        return streamBuilderArgs.StreamType switch
        {
            StreamType.Monitoring => new MonitoringStream
                                     (
                                        streamBuilderArgs.UserId,
                                        await _mediator.Send(new GetAvailableMiningCombinationsQuery(streamBuilderArgs.UserId)),
                                        streamBuilderArgs.ConnectionId,
                                        _userRigsObserverAggregator
                                     ),

            StreamType.Devices => throw new NotImplementedException(),
            StreamType.Rigs => throw new NotImplementedException(),
            StreamType.Statistics => throw new NotImplementedException(),
            _ => throw new NotImplementedException(),
        };
    }

    public UnionStreamBuilder(IUserRigsObserverAggregator userRigsObserverAggregator,
                              IMediator mediator)
    {
        _userRigsObserverAggregator = userRigsObserverAggregator;
        _mediator = mediator 
            ?? throw new ArgumentNullException(nameof(mediator));
    }
}