using MNX.MonitoringCenter.RigsApi.UnionStreams.Args;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

public interface IUnionStreamBuilder
{
    public Stream Build(UnionStreamBuilderArgs streamBuilderArgs);
}