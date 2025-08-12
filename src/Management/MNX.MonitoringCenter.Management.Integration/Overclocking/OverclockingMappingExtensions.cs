using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.Contracts.Overclocking;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Inventory;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.Overclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Cpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Inventory.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Cpu;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Overclocking;
using MNX.MonitoringCenter.Management.UseCases.SetRigDevices;

namespace MNX.MonitoringCenter.Management.Integration.Overclocking;

using AmdGpuOverclockingInventory = Inventory.Contracts.Devices.Gpu.Overclocking.AmdGpuOverclocking;
using CpuOverclockingInventory = Inventory.Contracts.Devices.Cpu.CpuOverclocking;
using IntelGpuOverclockingInventory = Inventory.Contracts.Devices.Gpu.Overclocking.IntelGpuOverclocking;
using NvidiaGpuOverclockingInventory = Inventory.Contracts.Devices.Gpu.Overclocking.NvidiaGpuOverclocking;
using OverclockingInventory = Inventory.Contracts.Devices.Overclocking;

/// <summary>
/// Расширение регистрации DI для маппинга сущностей разгона. 
/// </summary>
public static class OverclockingMappingExtensions
{
    ///
    public static IServiceCollection AddOverclockingMapping(this IServiceCollection services)
    {
        services.AddScoped<IOverclockingModelMapper<IOverclockingModel, IOverclocking>>(x =>
        {
            var registry = x.GetRequiredService<OverclockingModelMapperRegistry>();
            return new OverclockingModelMapperRegistryWrapper(registry);
        });

        services.AddScoped<IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking>, CpuOverclockingModelMapper>();
        services.AddScoped<IOverclockingModelMapper<NvidiaGpuOverclockingModel, NvidiaGpuOverclocking>, NvidiaGpuOverclockingModelMapper>();
        services.AddScoped<IOverclockingModelMapper<AmdGpuOverclockingModel, AmdGpuOverclocking>, AmdGpuOverclockingModelMapper>();
        services.AddScoped<IOverclockingModelMapper<IntelGpuOverclockingModel, IntelGpuOverclocking>, IntelGpuOverclockingModelMapper>();

        services.AddScoped(x =>
        {
            var registry = new OverclockingModelMapperRegistry();

            registry.Register(x.GetRequiredService<IOverclockingModelMapper<CpuOverclockingModel, CpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingModelMapper<AmdGpuOverclockingModel, AmdGpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingModelMapper<NvidiaGpuOverclockingModel, NvidiaGpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingModelMapper<IntelGpuOverclockingModel, IntelGpuOverclocking>>());

            return registry;
        });

        services.AddScoped<IOverclockingInventoryMapper<OverclockingInventory, IOverclocking>>(x =>
        {
            var registry = x.GetRequiredService<OverclockingInventoryMapperRegistry>();
            return new OverclockingInventoryMapperRegistryWrapper(registry);
        });

        services.AddScoped<IOverclockingInventoryMapper<CpuOverclockingInventory, CpuOverclocking>, CpuOverclockingInventoryMapper>();
        services.AddScoped<IOverclockingInventoryMapper<NvidiaGpuOverclockingInventory, NvidiaGpuOverclocking>, NvidiaGpuOverclockingInventoryMapper>();
        services.AddScoped<IOverclockingInventoryMapper<AmdGpuOverclockingInventory, AmdGpuOverclocking>, AmdGpuOverclockingInventoryMapper>();
        services.AddScoped<IOverclockingInventoryMapper<IntelGpuOverclockingInventory, IntelGpuOverclocking>, IntelGpuOverclockingInventoryMapper>();

        services.AddScoped(x =>
        {
            var registry = new OverclockingInventoryMapperRegistry();

            registry.Register(x.GetRequiredService<IOverclockingInventoryMapper<CpuOverclockingInventory, CpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingInventoryMapper<NvidiaGpuOverclockingInventory, NvidiaGpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingInventoryMapper<AmdGpuOverclockingInventory, AmdGpuOverclocking>>());
            registry.Register(x.GetRequiredService<IOverclockingInventoryMapper<IntelGpuOverclockingInventory, IntelGpuOverclocking>>());

            return registry;
        });

        services.AddScoped<IOverclockingToFanOverclockingAgentMapper<IOverclocking, FanOverclocking>>(x =>
        {
            var registry = x.GetRequiredService<FanOverclockingAgentMapperRegistry>();
            return new OverclockingToFanOverclockingAgentMapper(registry);
        });

        return services;
    }
}
