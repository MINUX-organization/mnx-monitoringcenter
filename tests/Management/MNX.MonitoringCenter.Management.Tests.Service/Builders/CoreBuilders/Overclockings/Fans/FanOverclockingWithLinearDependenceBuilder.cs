using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithLinearDependenceBuilder
    : BaseFanOverclockingBuilder<FanOverclockingWithLinearDependenceBuilder, FanOverclockingWithLinearDependence>
{
    protected List<FanGraphicPoint> _targetPoints = new(0);

    public FanOverclockingWithLinearDependenceBuilder AddTargetPoint(
        Func<FanGraphicPointBuilder, FanGraphicPointBuilder> configure)
    {
        var builder = new FanGraphicPointBuilder();
        builder = configure(builder);
        _targetPoints.Add(builder.Build());
        return this;
    }

    public override FanOverclockingWithLinearDependence Build()
    {
        return new FanOverclockingWithLinearDependence
        {
            Id = _id,
            TargetPoints = [.. _targetPoints],
        };
    }
}
