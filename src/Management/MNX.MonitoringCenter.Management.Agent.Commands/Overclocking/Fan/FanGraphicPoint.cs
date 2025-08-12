namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;

/// <summary>
/// Точка графика целевых показателей AutoFan.
/// </summary>
public record FanGraphicPoint
{
    /// <summary>
    /// Индекс точки графика автоматического разгона вентилятора.
    /// </summary>
    public int PointIndex { get; set; }

    /// <summary>
    /// Значение точки графика относительно оси значений скорости вентилятора (в процентах %).
    /// </summary>
    public int FanSpeedValueTarget { get; init; }

    /// <summary>
    /// Значение точки графика относительно оси значений температуры (в градусах по Цельсию °C).
    /// </summary>
    public int TemperatureValueTarget { get; init; }
}
