using AutoMapper;
using MNX.Application.UseCases.DI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.DataAccess;
using MNX.MonitoringCenter.Management.Integration;
using MNX.MonitoringCenter.Management.DataAccess.Preset;
using MNX.MonitoringCenter.Management.DataAccess.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets;
using MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries;
using MNX.MonitoringCenter.Management.Agent.Commands.Mining.ApplySettings;
using MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Commands.SavePreset;

namespace MNX.MonitoringCenter.Management.UseCases.IntegrationTests;

public abstract class BaseTest
{
    public ServiceProvider ServiceProvider { get; private set; }
    private string _dbPath;

    [OneTimeSetUp]
    public void SetUp()
    {
        var folder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        var testFolder = $"{folder}{Path.DirectorySeparatorChar}MNX.MonitoringCenter.Inventory.IntegrationTests";
        if (!Directory.Exists(testFolder))
            Directory.CreateDirectory(testFolder);

        _dbPath = $"{testFolder}{Path.DirectorySeparatorChar}{GetType().Name}.db";

        var services = new ServiceCollection();

        services.AddUseCaseMappingProfile();
        services.AddAutoMapper(cfg => cfg.AddProfiles(new List<Profile>()
        {
            new DbMappingProfile()
        }));

        services.AddLogging();

        services.AddDbContextFactory<Context>(delegate (DbContextOptionsBuilder options)
        {
            options.UseSqlite($"Data Source={_dbPath}");
            options.UseSnakeCaseNamingConvention();
        });

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(GetAvailableAlgorithmsQuery).Assembly,
            typeof(ApplyWorkerSettingsCommand).Assembly
        ));
        services.AddValidationPipelines(typeof(SavePresetValidator).Assembly);

        services.AddScoped<IRigRepository, RigRepository>();
        services.AddScoped<IMiningDeviceRepository, MiningDeviceRepository>();
        services.AddScoped<IPresetRepository, PresetRepository>();

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
