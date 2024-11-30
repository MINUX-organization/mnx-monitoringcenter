using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

public interface IUnionStreamBuilder
{
    public Task<Stream> Build(UnionStreamBuilderArgs streamBuilderArgs);
}