using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.Data.DI;
using MNX.Application.RabbitMQ;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rig;
using MNX.MonitoringCenter.Inventory.Controllers;
using MNX.MonitoringCenter.Inventory.DataAccess;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Drive;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Motherboard;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory;
using MNX.MonitoringCenter.Inventory.UseCases.Software;
using System.Reflection;

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
        services.AddEasyNetQ(configuration, new Assembly[] { typeof(RigConsumer).Assembly });

        services.AddMediatR(x => x.RegisterServicesFromAssemblies(
            typeof(SaveRigInventoryCommand).Assembly,
            typeof(SaveRigInventoryCommandHandler).Assembly
            ));
        services.AddValidationPipelines(typeof(SaveRigInventoryCommand).Assembly);

        services.AddDataContext<Context>(configuration);

        services.AddScoped<ICpuRepository, InventoryRepository>()
                .AddScoped<IDriveRepository, InventoryRepository>()
                .AddScoped<IGpuRepository, InventoryRepository>()
                .AddScoped<IMotherboardRepository, InventoryRepository>()
                .AddScoped<INetworkAdapterRepository, InventoryRepository>()
                .AddScoped<ISoftwareRepository, InventoryRepository>()
                .AddScoped<IRigRepository, RigRepository>();

        return services;
    }
}
