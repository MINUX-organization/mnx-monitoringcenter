using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

public class FanOverclockingWithTargetSpeedModelBuilder :
    BaseFanOverclockingModelBuilder<FanOverclockingWithTargetSpeedModelBuilder, FanOverclockingWithTargetSpeedModel>
{
    protected int _targetSpeed = 0;

    public FanOverclockingWithTargetSpeedModelBuilder WithTargetSpeed(int targetSpeed)
    {
        _targetSpeed = targetSpeed;
        return this;
    }

    public override FanOverclockingWithTargetSpeedModel Build()
    {
        return new FanOverclockingWithTargetSpeedModel
        {
            TargetSpeed = _targetSpeed,
        };
    }
}
