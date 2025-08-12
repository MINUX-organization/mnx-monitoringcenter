using MessagePack;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Overclocking.Fan;

/// <summary>
/// Разгон скорости вентилятора.
/// </summary>
[Union(0, typeof(FanOverclockingWithTargetSpeed))]
[Union(1, typeof(FanOverclockingWithTargetTemperature))]
[Union(2, typeof(FanOverclockingWithLinearDependence))]
public abstract record FanOverclocking { }
