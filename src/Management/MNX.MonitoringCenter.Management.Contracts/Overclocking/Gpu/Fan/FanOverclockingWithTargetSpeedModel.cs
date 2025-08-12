using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

/// <summary>
/// Модель разгона скорости вентилятора,
/// задаваемой целевым значением скорости вентилятора.
/// </summary>
public class FanOverclockingWithTargetSpeedModel : IFanOverclockingModel
{
    /// <inheritdoc/>
    [JsonIgnore]
    public FanOverclockingType FanOverclockingType => FanOverclockingType.TargetSpeed;

    /// <summary>
    /// Целевое значение разгона вентилятора (в процентах %).
    /// </summary>
    public int TargetSpeed { get; set; }
}
