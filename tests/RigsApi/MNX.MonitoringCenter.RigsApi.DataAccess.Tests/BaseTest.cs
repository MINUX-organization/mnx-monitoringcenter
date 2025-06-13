using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.RigsApi.Core.Services;
using NUnit.Framework;
using System.Reflection;

namespace MNX.MonitoringCenter.RigsApi.DataAccess.Tests;

public abstract class BaseTest
{
    private string _dbPath;

    public ServiceProvider ServiceProvider { get; private set; }

    [OneTimeSetUp]
    public void Setup()
    {
        // db
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var testFolder = $"{folder}{Path.DirectorySeparatorChar}{Assembly.GetExecutingAssembly().GetName()}";
        if (!Directory.Exists(testFolder))
            Directory.CreateDirectory(testFolder);

        _dbPath = $"{testFolder}{Path.DirectorySeparatorChar}{GetType().Name}.db";

        var services = new ServiceCollection();

        services.AddDbContext<Context>(delegate (DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
            options.UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IRigRepository, RigRepository>();

        ServiceProvider = services.BuildServiceProvider();

        ServiceProvider.GetRequiredService<Context>().Database.EnsureDeleted();
        ServiceProvider.GetRequiredService<Context>().Database.EnsureCreated();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceProvider.GetRequiredService<Context>().Database.EnsureDeleted();
        ServiceProvider.Dispose();
    }
}
