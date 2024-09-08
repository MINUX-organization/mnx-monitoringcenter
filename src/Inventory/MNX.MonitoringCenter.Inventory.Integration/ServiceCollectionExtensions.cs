using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.Data.DI;
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

namespace MNX.MonitoringCenter.Inventory.Integration;

/// <summary>
/// Расширения для <see cref="IServiceCollection"/>.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Добавить модуль инвентаризации.
    /// </summary>
    /// <param name="services"> DI. </param>
    /// <param name="configuration"> Конфигурация. </param>
    /// <returns> DI. </returns>
    public static IServiceCollection AddInventoryModule(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMediatR(x => x.RegisterServicesFromAssembly(typeof(SaveInventoryCommand).Assembly));
        services.AddValidationPipelines(typeof(SaveInventoryCommand).Assembly);

        services.AddDataContext<Context>(configuration);

        services.AddScoped<ICpuRepository, CpuRepository>()
                .AddScoped<IDriveRepository, DriveRepository>()
                .AddScoped<IGpuRepository, GpuRepository>()
                .AddScoped<IMotherboardRepository, MotherboardRepository>()
                .AddScoped<INetworkAdapterRepository, NetworkAdapterRepository>()
                .AddScoped<ISoftwareRepository, SoftwareRepository>()
                .AddScoped<IInventoryRepository, InventoryRepository>();

        return services;
    }
}
