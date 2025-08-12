namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;

/// <summary>
/// Разгон скорости вентилятора, задаваемый целевой температурой.
/// </summary>
public record FanOverclockingWithTargetTemperature : FanOverclocking
{
    /// <summary>
    /// Минимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int MinTargetSpeed { get; set; }

    /// <summary>
    /// Максимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int MaxTargetSpeed { get; set; }

    /// <summary>
    /// Целевое значение температуры процессора (в градусах по Цельсию °C).
    /// </summary>
    public int TargetCoreTemperature { get; set; }

    /// <summary>
    /// Целевое значение температуры памяти (в градусах по Цельсию °C).
    /// </summary>
    public int TargetMemoryTemperature { get; set; }
}
