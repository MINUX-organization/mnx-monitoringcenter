using MediatR;
using MNX.Application.UseCases.Mediator;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.UseCases.Mining;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;
using MNX.MonitoringCenter.RigsApi.UnionStreams.Streams;
using MNX.MonitoringCenter.Traffic.Observers.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Streams;

public class UnionStreamBuilder : IUnionStreamBuilder
{
    private readonly IUserRigsObserverAggregator _userRigsObserverAggregator;
    private readonly IMediator _mediator;

    public async Task<UnionStreams.Abstractions.Stream> Build(UnionStreamBuilderArgs streamBuilderArgs)
    {
        return streamBuilderArgs.StreamType switch
        {
            StreamType.Monitoring => new MonitoringStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                await _mediator.Send(new GetMiningCombinationsQuery(streamBuilderArgs.UserId)),
                await _mediator.GetListAsync(new GetRigsDetailsQuery(streamBuilderArgs.UserId), default),
                _userRigsObserverAggregator),

            StreamType.Devices => new DevicesStream(
                streamBuilderArgs.UserId, 
                streamBuilderArgs.ConnectionId, 
                _mediator.CreateStream(new GetCpusDetailsQuery(streamBuilderArgs.UserId)),
                _mediator.CreateStream(new GetGpusDetailsQuery(streamBuilderArgs.UserId)),
                await _mediator.Send(new GetMiningCombinationsQuery(streamBuilderArgs.UserId)),
                _userRigsObserverAggregator),

            StreamType.Rigs => new RigsStream(
                streamBuilderArgs.UserId,
                streamBuilderArgs.ConnectionId,
                await _mediator.GetListAsync(new GetRigsQuery(streamBuilderArgs.UserId), default),
                _userRigsObserverAggregator),

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