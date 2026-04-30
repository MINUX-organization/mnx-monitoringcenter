using MediatR;
using MNX.MonitoringCenter.Management.UseCases;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests.ApplyWorkerSettingsCommandResultConsumer;

internal class MiningDeviceStateChangedEventHandler : INotificationHandler<MiningDeviceStateChangedEvent>
{
    public static readonly List<MiningDeviceStateChangedEvent> Events = [];

    public Task Handle(MiningDeviceStateChangedEvent notification, CancellationToken cancellationToken)
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
