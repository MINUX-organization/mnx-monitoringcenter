using EasyNetQ;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;

namespace MNX.MonitoringCenter.RigsApi.RigConsumer_Client;

public class Program
{
    static async Task Main(string[] args)
    {
        await Task.WhenAll(SendFlightSheetSettingsConfirmation());
    }

    private static async Task SendFlightSheetSettingsConfirmation()
    {
        using var bus = RabbitHutch.CreateBus("host=77.37.200.24:5672;username=guest;password=guest;publisherConfirms=true");

        await bus.PubSub.PublishAsync(new ApplyWorkerSettingsCommandResult(
            new List<Guid>(),
            new List<Guid>()
            ));
    }
}