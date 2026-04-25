using Testcontainers.RabbitMq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

public class RabbitFixture
{
    public RabbitMqContainer _container;

    public string Host => _container.Hostname;
    public int Port => _container.GetMappedPublicPort(5672);

    public async Task StartAsync()
    {
        _container = new RabbitMqBuilder("rabbitmq:3-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await _container.StartAsync();
    }

    public async Task StopAsync()
    {
        await _container.DisposeAsync();
    }
}
