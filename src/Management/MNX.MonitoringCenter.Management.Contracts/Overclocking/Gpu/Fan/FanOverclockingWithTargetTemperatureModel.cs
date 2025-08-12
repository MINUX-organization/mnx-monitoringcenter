using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

/// <summary>
/// Реализация <see cref="IFanOverclocking"/>.
/// Разгон вентилятора видеокарты по целевому значению температуры видеокарты.
/// </summary>
public class FanOverclockingWithTargetTemperatureModel : IFanOverclockingModel
{
    /// <inheritdoc/>
    [JsonIgnore]
    public FanOverclockingType FanOverclockingType => FanOverclockingType.TargetTemperature;

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
