using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithTargetSpeedBuilder :
    BaseFanOverclockingBuilder<FanOverclockingWithTargetSpeedBuilder, FanOverclockingWithTargetSpeed>
{
    protected int _targetSpeed = 0;

    public FanOverclockingWithTargetSpeedBuilder WithTargetSpeed(int targetSpeed)
    {
        _targetSpeed = targetSpeed;
        return this;
    }

    public override FanOverclockingWithTargetSpeed Build()
    {
        return new FanOverclockingWithTargetSpeed
        {
            Id = _id,
            TargetSpeed = _targetSpeed,
        };
    }
}
