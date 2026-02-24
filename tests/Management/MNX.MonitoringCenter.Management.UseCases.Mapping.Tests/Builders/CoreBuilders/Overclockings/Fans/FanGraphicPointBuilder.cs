using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.UseCases.Mapping.Tests.Builders.CoreBuilders.Overclockings.Fans;

public class FanGraphicPointBuilder
{
    private int _pointIndex = 0;
    private int _fanSpeedValueTarget = 0;
    private int _temperatureValueTarget = 0;

    public FanGraphicPointBuilder WithFanSpeedValueTarget(int fanSpeedValueTarget)
    {
        _fanSpeedValueTarget = fanSpeedValueTarget;
        return this;
    }

    public FanGraphicPointBuilder WithPointIndex(int pointIndex)
    {
        _pointIndex = pointIndex;
        return this;
    }

    public FanGraphicPointBuilder WithTemperatureValueTarget(int temperatureValueTarget)
    {
        _temperatureValueTarget = temperatureValueTarget;
        return this;
    }

    public FanGraphicPoint Build()
    {
        return new FanGraphicPoint
        {
            FanSpeedValueTarget = _fanSpeedValueTarget,
            PointIndex = _pointIndex,
            TemperatureValueTarget = _temperatureValueTarget,
        };
    }
}
