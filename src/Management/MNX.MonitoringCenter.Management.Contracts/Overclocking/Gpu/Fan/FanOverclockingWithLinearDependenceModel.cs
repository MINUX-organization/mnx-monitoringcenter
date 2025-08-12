using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;

/// <summary>
/// Реализация <see cref="IFanOverclocking"/>.
/// Разгон вентилятора видеокарты по целевым значениям
/// графических точек скорости вентилятора в зависимости от температуры видеокарты.
/// </summary>
public class FanOverclockingWithLinearDependenceModel : IFanOverclockingModel
{
    /// <inheritdoc/>
    [JsonIgnore]
    public FanOverclockingType FanOverclockingType => FanOverclockingType.LinearDependence;

    /// <summary>
    /// Точки графика целевых показателей скорости вентилятора
    /// в зависимости от температуры устройства.
    /// </summary>
    public FanGraphicPointModel[] TargetPoints { get; set; } = [];
}
