namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;

/// <summary>
/// Разгон скорости вентилятора, задаваемый графиком.
/// </summary>
public record FanOverclockingWithLinearDependence : FanOverclocking
{
    /// <summary>
    /// Точки графика целевых показателей скорости вентилятора
    /// в зависимости от температуры устройства.
    /// </summary>
    public FanGraphicPoint[] TargetPoints { get; set; } = [];
}
