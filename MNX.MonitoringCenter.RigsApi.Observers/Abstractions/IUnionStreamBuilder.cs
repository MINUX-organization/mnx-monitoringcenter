using MNX.MonitoringCenter.RigsApi.Args;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

public interface IUnionStreamBuilder
{
    public Task<Stream> Build(UnionStreamBuilderArgs streamBuilderArgs);
}