using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.Tests.Service.Builders.ContractBuilders.Overclockings.Fan;

public class FanGraphicPointModelBuilder
{
    private int _fanSpeedValueTarget = 0;
    private int _temperatureValueTarget = 0;

    public FanGraphicPointModelBuilder WithFanSpeedValueTarget(int fanSpeedValueTarget)
    {
        _fanSpeedValueTarget = fanSpeedValueTarget;
        return this;
    }

    public FanGraphicPointModelBuilder WithTemperatureValueTarget(int temperatureValueTarget)
    {
        _temperatureValueTarget = temperatureValueTarget;
        return this;
    }

    public FanGraphicPointModel Build()
    {
        return new FanGraphicPointModel
        {
            FanSpeedValueTarget = _fanSpeedValueTarget,
            TemperatureValueTarget = _temperatureValueTarget,
        };
    }
}
