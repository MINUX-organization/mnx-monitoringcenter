using Testcontainers.PostgreSql;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests;

public static class IntegrationTestContainer
{
    private static readonly object _lock = new object();
    private static PostgreSqlContainer? _container;

    public static PostgreSqlContainer Container
    {
        get
        {
            if (_container is null)
                throw new InvalidOperationException("Container is not started yet");
            return _container;
        }
    }

    public static async Task StartAsync()
    {
        if (_container is not null)
            return;

        lock (_lock)
        {
            _container ??= new PostgreSqlBuilder("postgres:16.2")
                .WithDatabase("monitoring_center")
                .WithUsername("minux_admin")
                .WithPassword("t6y12_!hgx2")
                .Build();
        }

        await _container.StartAsync();
    }

    public static async Task StopAsync()
    {
        if (Container is not null)
        {
            await Container.StopAsync();
            _container = null;
        }
    }
}
