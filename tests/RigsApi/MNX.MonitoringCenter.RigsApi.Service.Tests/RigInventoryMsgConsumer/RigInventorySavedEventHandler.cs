using MediatR;
using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.RigInventoryMsgConsumer;

internal class RigInventorySavedEventHandler : INotificationHandler<RigInventorySavedEvent>
{
    public static readonly List<RigInventorySavedEvent> Events = [];

    public Task Handle(RigInventorySavedEvent notification, CancellationToken cancellationToken)
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
