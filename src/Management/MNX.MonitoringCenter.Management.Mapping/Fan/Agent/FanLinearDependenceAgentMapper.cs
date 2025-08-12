using MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;
using MNX.MonitoringCenter.Management.UseCases.Mapping.Overclocking.Agent;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Fan.Agent;

using FanOverclockingWithLinearDependenceAgent = FanOverclockingWithLinearDependence;
using FanOverclockingWithLinearDependenceCore = Core.Overclocking.Gpu.Fan.FanOverclockingWithLinearDependence;

/// <summary>
/// Реализация <see cref="IFanOverclockingAgentMapper{TModel, TEntity}"/> для сущностей
/// <see cref="FanOverclockingWithLinearDependenceAgent"/> и <see cref="FanOverclockingWithLinearDependenceCore"/>.
/// </summary>
public class FanLinearDependenceAgentMapper : IFanOverclockingAgentMapper<FanOverclockingWithLinearDependenceAgent, FanOverclockingWithLinearDependenceCore>
{
    /// <inheritdoc/>
    public FanOverclocking MapToModel(FanOverclockingWithLinearDependenceCore entity)
    {
        return new FanOverclockingWithLinearDependenceAgent()
        {
            TargetPoints = entity.TargetPoints.Select(x => new FanGraphicPoint()
            {
                PointIndex = x.PointIndex,
                FanSpeedValueTarget = x.FanSpeedValueTarget,
                TemperatureValueTarget = x.TemperatureValueTarget
            }).ToArray(),
        };
    }
}
