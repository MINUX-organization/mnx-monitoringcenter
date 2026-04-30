using MediatR;
using MNX.MonitoringCenter.RigsApi.Core.DomainEvents;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

internal abstract class BaseDomainEventHandler<T> : INotificationHandler<T>
    where T : BaseDomainEvent
{
    public static readonly List<BaseDomainEvent> Events = [];

    public Task Handle(T notification, CancellationToken cancellationToken)
    {
        Events.Add(notification);
        return Task.CompletedTask;
    }

    public static Task ClearEvents()
    {
        Events.Clear();
        return Task.CompletedTask;
    }
}
