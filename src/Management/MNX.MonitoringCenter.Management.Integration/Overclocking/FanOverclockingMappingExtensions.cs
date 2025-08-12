using Microsoft.Extensions.DependencyInjection;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Infrastructure.FanOverclocking.Model;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Model;

namespace MNX.MonitoringCenter.Management.Integration.Overclocking;

using FanOverclockingWithLinearDependenceAgent = Agent.Commands.Overclocking.Fan.FanOverclockingWithLinearDependence;
using FanOverclockingWithLinearDependenceCore = FanOverclockingWithLinearDependence;
using FanOverclockingWithTargetSpeedAgent = Agent.Commands.Overclocking.Fan.FanOverclockingWithTargetSpeed;
using FanOverclockingWithTargetSpeedCore = FanOverclockingWithTargetSpeed;
using FanOverclockingWithTargetTemperatureAgent = Agent.Commands.Overclocking.Fan.FanOverclockingWithTargetTemperature;
using FanOverclockingWithTargetTemperatureCore = FanOverclockingWithTargetTemperature;

/// <summary>
/// Расширение регистрации DI для маппинга сущности разгона вентилятора.
/// </summary>
public static class FanOverclockingMappingExtensions
{
    ///
    public static IServiceCollection AddFanOverclockingMapping(this IServiceCollection services)
    {
        services.AddScoped<IFanOverclockingModelMapper<FanOverclockingWithLinearDependenceModel, FanOverclockingWithLinearDependenceCore>,
            FanLinearDependenceModelMapper>();
        services.AddScoped<IFanOverclockingModelMapper<FanOverclockingWithTargetSpeedModel, FanOverclockingWithTargetSpeedCore>,
            FanTargetSpeedModelMapper>();
        services.AddScoped<IFanOverclockingModelMapper<FanOverclockingWithTargetTemperatureModel, FanOverclockingWithTargetTemperatureCore>,
            FanTargetTemperatureModelMapper>();

        services.AddScoped(x =>
        {
            var registry = new FanOverclockingModelMapperRegistry();

            registry.Register(x.GetRequiredService<IFanOverclockingModelMapper<FanOverclockingWithLinearDependenceModel,
                FanOverclockingWithLinearDependenceCore>>());
            registry.Register(x.GetRequiredService<IFanOverclockingModelMapper<FanOverclockingWithTargetSpeedModel,
                FanOverclockingWithTargetSpeedCore>>());
            registry.Register(x.GetRequiredService<IFanOverclockingModelMapper<FanOverclockingWithTargetTemperatureModel,
                FanOverclockingWithTargetTemperatureCore>>());

            return registry;
        });


        services.AddScoped<IFanOverclockingAgentMapper<FanOverclockingWithLinearDependenceAgent, FanOverclockingWithLinearDependenceCore>,
            FanLinearDependenceAgentMapper>();
        services.AddScoped<IFanOverclockingAgentMapper<FanOverclockingWithTargetSpeedAgent, FanOverclockingWithTargetSpeedCore>,
            FanTargetSpeedAgentMapper>();
        services.AddScoped<IFanOverclockingAgentMapper<FanOverclockingWithTargetTemperatureAgent, FanOverclockingWithTargetTemperatureCore>,
            FanTargetTemperatureAgentMapper>();

        services.AddScoped(x =>
        {
            var registry = new FanOverclockingAgentMapperRegistry();

            registry.Register(x.GetRequiredService<IFanOverclockingAgentMapper<FanOverclockingWithLinearDependenceAgent,
                FanOverclockingWithLinearDependenceCore>>());
            registry.Register(x.GetRequiredService<IFanOverclockingAgentMapper<FanOverclockingWithTargetSpeedAgent,
                FanOverclockingWithTargetSpeedCore>>());
            registry.Register(x.GetRequiredService<IFanOverclockingAgentMapper<FanOverclockingWithTargetTemperatureAgent,
                FanOverclockingWithTargetTemperatureCore>>());

            return registry;
        });

        return services;
    }
}
