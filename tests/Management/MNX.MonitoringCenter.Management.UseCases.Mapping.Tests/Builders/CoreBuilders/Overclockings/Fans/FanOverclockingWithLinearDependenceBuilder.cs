using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithLinearDependenceBuilder
    : BaseFanOverclockingBuilder<FanOverclockingWithLinearDependenceBuilder, FanOverclockingWithLinearDependence>
{
    protected List<FanGraphicPoint> _targetPoints = new(0);

    public FanOverclockingWithLinearDependenceBuilder AddTargetPoint(Action<FanGraphicPointBuilder> configure)
    {
        var builder = new FanGraphicPointBuilder();
        configure(builder);
        _targetPoints.Add(builder.Build());
        return this;
    }

    public override IFanOverclocking Build()
    {
        return new FanOverclockingWithLinearDependence
        {
            Id = _id,
            TargetPoints = [.. _targetPoints],
        };
    }
}
