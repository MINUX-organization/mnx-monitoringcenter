using Respawn;
using Microsoft.EntityFrameworkCore;
using System.Data.Common;

namespace MNX.MonitoringCenter.Management.DataAccess.Tests.Infrastructure;

public abstract class BaseTest
{
    private static Respawner _respawner;
    private static DbConnection _connection;

    protected Context Context;

    [OneTimeSetUp]
    public async Task OneTimeSetUp()
    {
        await IntegrationTestContainer.StartAsync();

        var connectionString = IntegrationTestContainer.Container.GetConnectionString();

        var options = new DbContextOptionsBuilder<Context>()
            .UseNpgsql(connectionString)
            .Options;
        Context = new Context(options);

        _connection = Context.Database.GetDbConnection();
        await _connection.OpenAsync();

        await Context.Database.EnsureCreatedAsync();

        _respawner ??= await Respawner.CreateAsync(_connection, new RespawnerOptions
        {
            DbAdapter = DbAdapter.Postgres
        });
    }

    [SetUp]
    public async Task ResetDatabase()
    {
        if (_respawner is not null && _connection is not null)
        {
            await _respawner.ResetAsync(_connection);
            Context.ChangeTracker.Clear();
        }
    }

    [OneTimeTearDown]
    public async Task OneTimeTearDown()
    {
        if (Context is not null)
        {
            await Context.DisposeAsync();
            await _connection.DisposeAsync();
        }

        await IntegrationTestContainer.StopAsync();
    }
}
