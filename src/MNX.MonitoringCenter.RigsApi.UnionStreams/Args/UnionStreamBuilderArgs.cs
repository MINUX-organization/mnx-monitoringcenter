using MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Args;

public record UnionStreamBuilderArgs(Guid UserId, string ConnectionId, StreamType StreamType);