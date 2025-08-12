namespace MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

/// <summary>
/// Точка графика целевых показателей AutoFan.
/// </summary>
public class FanGraphicPoint : ICloneable
{
    /// <summary>
    /// Индекс точки графика автоматического разгона вентилятора.
    /// </summary>
    public int PointIndex { get; set; }

    /// <summary>
    /// Значение точки графика относительно оси значений скорости вентилятора (в процентах %).
    /// </summary>
    public int FanSpeedValueTarget { get; set; }

    /// <summary>
    /// Значение точки графика относительно оси значений температуры (в градусах по Цельсию °C).
    /// </summary>
    public int TemperatureValueTarget { get; set; }

    /// <inheritdoc/>
    public object Clone()
    {
        return new FanGraphicPoint()
        {
            FanSpeedValueTarget = FanSpeedValueTarget,
            TemperatureValueTarget = TemperatureValueTarget
        };
    }
}