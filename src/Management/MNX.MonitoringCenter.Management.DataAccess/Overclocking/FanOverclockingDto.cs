using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using MNX.MonitoringCenter.Management.Core.Overclocking.Gpu.Fan;

namespace MNX.MonitoringCenter.Management.DataAccess.Overclocking;

/// <summary>
/// Разгон вентилятора.
/// </summary>
public class FanOverclockingDto
{
    /// <summary>
    /// Первичный ключ.
    /// </summary>
    public Guid Id { get; init; } = Guid.NewGuid();

    /// <summary>
    /// Тип разгона вентилятора.
    /// </summary>
    public FanOverclockingType Type { get; init; }

    /// <summary>
    /// Целевое значение разгона вентилятора (в процентах %).
    /// </summary>
    public int? TargetSpeed { get; init; }

    /// <summary>
    /// Минимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int? MinTargetSpeed { get; init; }

    /// <summary>
    /// Максимальная целевая скорость вентилятора (в процентах %).
    /// </summary>
    public int? MaxTargetSpeed { get; init; }

    /// <summary>
    /// Целевое значение температуры процессора (в градусах по Цельсию °C).
    /// </summary>
    public int? TargetCoreTemperature { get; init; }

    /// <summary>
    /// Целевое значение температуры памяти (в градусах по Цельсию °C).
    /// </summary>
    public int? TargetMemoryTemperature { get; init; }

    /// <summary>
    /// Точки графика целевых показателей скорости вентилятора
    /// в зависимости от температуры устройства.
    /// </summary>
    public FanGraphicPoint[]? TargetPoints { get; init; }
}
