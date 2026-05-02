using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

public class FanOverclockingWithLinearDependenceModelBuilder
    : BaseFanOverclockingModelBuilder<FanOverclockingWithLinearDependenceModelBuilder, FanOverclockingWithLinearDependenceModel>
{
    protected readonly List<FanGraphicPointModel> _targetPoints = [];

    public FanOverclockingWithLinearDependenceModelBuilder AddTargetPoint(
        Func<FanGraphicPointModelBuilder, FanGraphicPointModelBuilder>? configure = null)
    {
        var builder = new FanGraphicPointModelBuilder();
        builder = configure?.Invoke(builder) ?? builder;
        _targetPoints.Add(builder.Build());
        return this;
    }

    public FanOverclockingWithLinearDependenceModelBuilder WithTargetPoints(Func<List<FanGraphicPointModel>> factory)
    {
        _targetPoints.AddRange(factory());
        return this;
    }

    public override FanOverclockingWithLinearDependenceModel Build()
    {
        return new FanOverclockingWithLinearDependenceModel
        {
            TargetPoints = _targetPoints.ToArray(),
        };
    }
}
