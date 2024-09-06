using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.DataAccess;
using MNX.MonitoringCenter.Inventory.DataAccess.Cpu;
using MNX.MonitoringCenter.Inventory.DataAccess.Drive;
using MNX.MonitoringCenter.Inventory.DataAccess.Gpu;
using MNX.MonitoringCenter.Inventory.DataAccess.Motherboard;
using MNX.MonitoringCenter.Inventory.DataAccess.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.DataAccess.Software;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.Software;
using NUnit.Framework;

namespace MNX.MonitoringCenter.Inventory.IntegrationTests;

public abstract class BaseTest
{
    public ServiceProvider ServiceProvider { get; private set; }
    private string _dbPath;

    [OneTimeSetUp]
    public void Setup()
    {
        // db
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var testFolder = $"{folder}{Path.DirectorySeparatorChar}MNX.MonitoringCenter.Inventory.IntegrationTests";
        if (!Directory.Exists(testFolder))
            Directory.CreateDirectory(testFolder);

        _dbPath = $"{testFolder}{Path.DirectorySeparatorChar}{GetType().Name}.db";

        var services = new ServiceCollection();

        services.AddDbContext<Context>(delegate (DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(SaveInventoryCommand).Assembly));
        services.AddValidationPipelines(typeof(SaveInventoryCommandValidator).Assembly);

        services.AddScoped<ICpuRepository, CpuRepository>()
                .AddScoped<IDriveRepository, DriveRepository>()
                .AddScoped<IGpuRepository, GpuRepository>()
                .AddScoped<IMotherboardRepository, MotherboardRepository>()
                .AddScoped<INetworkAdapterRepository, NetworkAdapterRepository>()
                .AddScoped<ISoftwareRepository, SoftwareRepository>()
                .AddScoped<IInventoryRepository, InventoryRepository>();

        ServiceProvider = services.BuildServiceProvider();

        ServiceProvider.GetRequiredService<Context>().Database.EnsureDeleted();
        ServiceProvider.GetRequiredService<Context>().Database.EnsureCreated();
    }

    [OneTimeTearDown]
    public void TearDown()
    {
        ServiceProvider.Dispose();
        ServiceProvider.GetRequiredService<Context>().Database.EnsureDeleted();
    }
}
