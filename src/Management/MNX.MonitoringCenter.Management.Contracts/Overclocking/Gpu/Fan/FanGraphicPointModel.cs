namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

/// <summary>
/// Модель целевой точки разгона вентилятора,
/// относительно температуры видеокарты на графике.
/// </summary>
public class FanGraphicPointModel
{
    /// <summary>
    /// Значение точки графика относительно
    /// оси значений скорости вентилятора (в процентах %).
    /// </summary>
    public int FanSpeedValueTarget { get; set; }

    /// <summary>
    /// Значение точки графика относительно
    /// оси значений температуры (в градусах по Цельсию °C).
    /// </summary>
    public int TemperatureValueTarget { get; set; }
}
