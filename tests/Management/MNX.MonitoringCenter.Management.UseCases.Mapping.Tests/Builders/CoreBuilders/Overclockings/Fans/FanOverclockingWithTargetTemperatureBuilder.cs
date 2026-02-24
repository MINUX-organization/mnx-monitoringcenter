using MNX.MonitoringCenter.Management.Core.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings.Fans;

public class FanOverclockingWithTargetTemperatureBuilder :
    BaseFanOverclockingBuilder<FanOverclockingWithTargetTemperatureBuilder, FanOverclockingWithTargetTemperature>
{
    protected int _maxTargetSpeed = 0;
    protected int _minTargetSpeed = 0;
    protected int _targetCoreTemperature = 0;
    protected int _targetMemoryTemperature = 0;

    public FanOverclockingWithTargetTemperatureBuilder WithMaxTargetSpeed(int maxTargetSpeed)
    {
        _maxTargetSpeed = maxTargetSpeed;
        return this;
    }

    public FanOverclockingWithTargetTemperatureBuilder WithMinTargetSpeed(int minTargetSpeed)
    {
        _minTargetSpeed = minTargetSpeed;
        return this;
    }

    public FanOverclockingWithTargetTemperatureBuilder WithTargetCoreTemperature(int targetCoreTemperature)
    {
        _targetCoreTemperature = targetCoreTemperature;
        return this;
    }

    public FanOverclockingWithTargetTemperatureBuilder WithTargetMemoryTemperature(int targetMemoryTemperature)
    {
        _targetMemoryTemperature = targetMemoryTemperature;
        return this;
    }

    public override IFanOverclocking Build()
    {
        return new FanOverclockingWithTargetTemperature
        {
            Id = _id,
            MaxTargetSpeed = _maxTargetSpeed,
            MinTargetSpeed = _minTargetSpeed,
            TargetCoreTemperature = _targetCoreTemperature,
            TargetMemoryTemperature = _targetMemoryTemperature,
        };
    }
}
