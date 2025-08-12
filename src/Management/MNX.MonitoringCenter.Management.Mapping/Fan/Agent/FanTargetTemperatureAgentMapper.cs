using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;

using FanOverclockingWithTargetTemperatureAgent = FanOverclockingWithTargetTemperature;
using FanOverclockingWithTargetTemperatureCore = Core.Overclocking.Gpu.Fan.FanOverclockingWithTargetTemperature;

/// <summary>
/// Реализация <see cref="IFanOverclockingAgentMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithTargetTemperatureAgent"/> и <see cref="FanOverclockingWithTargetTemperatureCore"/>.
/// </summary>
public class FanTargetTemperatureAgentMapper : IFanOverclockingAgentMapper<FanOverclockingWithTargetTemperatureAgent, FanOverclockingWithTargetTemperatureCore>
{
    /// <inheritdoc/>
    public FanOverclocking MapToModel(FanOverclockingWithTargetTemperatureCore entity)
    {
        return new FanOverclockingWithTargetTemperatureAgent()
        {
            MinTargetSpeed = entity.MinTargetSpeed,
            MaxTargetSpeed = entity.MaxTargetSpeed,
            TargetCoreTemperature = entity.TargetCoreTemperature,
            TargetMemoryTemperature = entity.TargetMemoryTemperature
        };
    }
}
