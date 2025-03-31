using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MNX.Application.Data.EF.DI;
using MNX.Application.UseCases.DI;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
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
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(
            typeof(GetRigsDetailsQuery).Assembly,
            typeof(SaveRigInventoryCommandHandler).Assembly
            ));
        services.AddValidationPipelines(typeof(SaveRigInventoryCommandHandler).Assembly);

        services.AddDataContext<Context>(configuration);

        services.AddAutoMapper(x => x.AddProfile<MappingProfile>());

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
