namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;

/// <summary>
/// Разгон вентилятора видеокарты по целевому значению скорости вентилятора.
/// </summary>
public record FanOverclockingWithTargetSpeed : FanOverclocking
{
    /// <summary>
    /// Целевое значение разгона вентилятора (в процентах %).
    /// </summary>
    public int TargetSpeed { get; set; }
}
