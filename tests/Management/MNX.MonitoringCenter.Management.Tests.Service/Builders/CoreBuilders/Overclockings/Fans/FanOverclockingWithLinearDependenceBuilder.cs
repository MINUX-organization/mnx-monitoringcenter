using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithLinearDependenceBuilder
    : BaseFanOverclockingBuilder<FanOverclockingWithLinearDependenceBuilder, FanOverclockingWithLinearDependence>
{
    private int _counter = 0;

    protected List<FanGraphicPoint> _targetPoints = new(0);

    public FanOverclockingWithLinearDependenceBuilder AddTargetPoint(
        Func<FanGraphicPointBuilder, FanGraphicPointBuilder>? configure = null)
    {
        var builder = new FanGraphicPointBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _targetPoints.Add(builder
            .WithPointIndex(_counter++)
            .Build());
        return this;
    }

    public FanOverclockingWithLinearDependenceBuilder AddTargetPoint(
        FanGraphicPoint point)
    {
        _targetPoints.Add(point);
        return this;
    }

    public FanOverclockingWithLinearDependenceBuilder WithTargets(
        Func<List<FanGraphicPoint>> factory)
    {
        _targetPoints.Clear();
        _targetPoints.AddRange(factory());
        return this;
    }

    public FanOverclockingWithLinearDependenceBuilder WithTargets(
        List<FanGraphicPoint> targetPoints)
    {
        _targetPoints.Clear();
        _targetPoints.AddRange(targetPoints);
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
