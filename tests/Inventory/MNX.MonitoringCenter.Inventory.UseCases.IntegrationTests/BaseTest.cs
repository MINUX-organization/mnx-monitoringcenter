using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;
using MNX.MonitoringCenter.Inventory.DataAccess;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Software;
using MNX.MonitoringCenter.Inventory.UseCases.Rigs;
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

        services.AddMediatR(x => x.RegisterServicesFromAssemblies(
            typeof(SaveRigInventoryCommand).Assembly,
            typeof(SaveRigInventoryCommandHandler).Assembly
            ));
        services.AddValidationPipelines(typeof(SaveRigInventoryCommandValidator).Assembly);

        services.AddScoped<ICpuRepository, InventoryRepository>()
                .AddScoped<IDriveRepository, InventoryRepository>()
                .AddScoped<IGpuRepository, InventoryRepository>()
                .AddScoped<IMotherboardRepository, InventoryRepository>()
                .AddScoped<INetworkAdapterRepository, InventoryRepository>()
                .AddScoped<ISoftwareRepository, InventoryRepository>()
                .AddScoped<IRigRepository, RigRepository>();

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
