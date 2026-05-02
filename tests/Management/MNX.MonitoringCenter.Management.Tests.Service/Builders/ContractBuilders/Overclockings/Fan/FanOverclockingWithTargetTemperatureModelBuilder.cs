using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

public class FanOverclockingWithTargetTemperatureModelBuilder :
    BaseFanOverclockingModelBuilder<FanOverclockingWithTargetTemperatureModelBuilder, FanOverclockingWithTargetTemperatureModel>
{
    protected int _maxTargetSpeed = 0;
    protected int _minTargetSpeed = 0;
    protected int _targetCoreTemperature = 0;
    protected int _targetMemoryTemperature = 0;

    public FanOverclockingWithTargetTemperatureModelBuilder WithMaxTargetSpeed(int maxTargetSpeed)
    {
        _maxTargetSpeed = maxTargetSpeed;
        return this;
    }

    public FanOverclockingWithTargetTemperatureModelBuilder WithMinTargetSpeed(int minTargetSpeed)
    {
        _minTargetSpeed = minTargetSpeed;
        return this;
    }

    public FanOverclockingWithTargetTemperatureModelBuilder WithTargetCoreTemperature(int targetCoreTemperature)
    {
        _targetCoreTemperature = targetCoreTemperature;
        return this;
    }

    public FanOverclockingWithTargetTemperatureModelBuilder WithTargetMemoryTemperature(int targetMemoryTemperature)
    {
        _targetMemoryTemperature = targetMemoryTemperature;
        return this;
    }

    public override FanOverclockingWithTargetTemperatureModel Build()
    {
        return new FanOverclockingWithTargetTemperatureModel
        {
            MaxTargetSpeed = _maxTargetSpeed,
            MinTargetSpeed = _minTargetSpeed,
            TargetCoreTemperature = _targetCoreTemperature,
            TargetMemoryTemperature = _targetMemoryTemperature,
        };
    }
}
