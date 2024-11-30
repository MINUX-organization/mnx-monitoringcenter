using MNX.MonitoringCenter.Traffic.Observers;
using System.Reactive.Subjects;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

public abstract class Stream
{
    protected abstract SubscriptionType[] SubscriptionTypes { get; }

    protected Subject<(SubscriptionType, object)> Subject { get; private set; } = new();

    public abstract IAsyncEnumerable<object> StartStreaming();

    public abstract void StopStreaming(Guid userId, string connectionId);
}
