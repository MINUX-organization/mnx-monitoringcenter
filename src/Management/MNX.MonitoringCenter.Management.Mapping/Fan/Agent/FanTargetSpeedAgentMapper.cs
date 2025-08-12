using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;

using FanOverclockingWithTargetSpeedAgent = FanOverclockingWithTargetSpeed;
using FanOverclockingWithTargetSpeedCore = Core.Overclocking.Gpu.Fan.FanOverclockingWithTargetSpeed;

/// <summary>
/// Реализация <see cref="IFanOverclockingAgentMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithTargetSpeedAgent"/> и <see cref="FanOverclockingWithTargetSpeedCore"/>.
/// </summary>
public class FanTargetSpeedAgentMapper : IFanOverclockingAgentMapper<FanOverclockingWithTargetSpeedAgent, FanOverclockingWithTargetSpeedCore>
{
    /// <inheritdoc/>
    public FanOverclocking MapToModel(FanOverclockingWithTargetSpeedCore entity)
    {
        return new FanOverclockingWithTargetSpeedAgent()
        {
            TargetSpeed = entity.TargetSpeed
        };
    }
}
