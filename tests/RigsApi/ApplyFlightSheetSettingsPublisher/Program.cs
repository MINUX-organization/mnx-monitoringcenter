using EasyNetQ;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;

namespace MNX.MonitoringCenter.RigsApi.RigConsumer_Client;

public class Program
{
    static async Task Main(string[] args)
    {
        await SendFlightSheetSettingsConfirmation();
    }

    private static async Task SendFlightSheetSettingsConfirmation()
    {
        using var bus = RabbitHutch.CreateBus("host=77.37.200.24:5672;username=guest;password=guest;publisherConfirms=true");

        await bus.PubSub.PublishAsync(new ApplyWorkerSettingsCommandResult(
            new List<Guid> { Guid.Parse("12b6f503-fbd1-5ad7-9548-26d79731c868") },
            new List<Guid>()
            ));
    }
}