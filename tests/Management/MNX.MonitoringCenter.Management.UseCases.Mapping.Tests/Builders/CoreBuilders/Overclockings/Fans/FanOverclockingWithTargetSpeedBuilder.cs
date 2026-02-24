using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithTargetSpeedBuilder :
    BaseFanOverclockingBuilder<FanOverclockingWithTargetSpeedBuilder, FanOverclockingWithTargetSpeed>
{
    private int _targetSpeed = 0;

    public FanOverclockingWithTargetSpeedBuilder WithTargetSpeed(int targetSpeed)
    {
        _targetSpeed = targetSpeed;
        return this;
    }

    public override IFanOverclocking Build()
    {
        return new FanOverclockingWithTargetSpeed
        {
            Id = _id,
            TargetSpeed = _targetSpeed,
        };
    }
}
