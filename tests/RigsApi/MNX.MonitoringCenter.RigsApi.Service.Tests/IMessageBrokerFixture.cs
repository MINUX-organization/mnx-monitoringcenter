using Testcontainers.RabbitMq;

namespace MNX.MonitoringCenter.RigsApi.Service.Tests;

public interface IMessageBrokerFixture : IAsyncDisposable
{
    string Host { get; }
    int Port { get; }
    Task StartAsync();
}

public class RabbitFixture : IMessageBrokerFixture
{
    private bool _disposedValue;
    private RabbitMqContainer? _container;

    public string Host => _container?.Hostname ??
        throw new ArgumentNullException(nameof(Host));

    public int Port => _container?.GetMappedPublicPort(5672) ??
        throw new ArgumentNullException(nameof(Port));

    public async Task StartAsync()
    {
        _container = new RabbitMqBuilder("rabbitmq:3-management")
            .WithUsername("guest")
            .WithPassword("guest")
            .Build();

        await _container.StartAsync();
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsync(disposing: true);
        GC.SuppressFinalize(this);
    }

    private async ValueTask DisposeAsync(bool disposing)
    {
        if (!_disposedValue)
        {
            if (disposing && _container is not null)
            {
                await _container.DisposeAsync();
            }

            _container = null!;
            _disposedValue = true;
        }
    }
}