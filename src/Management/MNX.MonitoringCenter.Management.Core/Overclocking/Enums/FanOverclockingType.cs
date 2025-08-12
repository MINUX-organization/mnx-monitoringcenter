namespace MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

/// <summary>
/// Тип разгона вентилятора.
/// </summary>
public enum FanOverclockingType
{
    /// <summary>
    /// Разгон по целевому значению скорости вентилятора.
    /// </summary>
    TargetSpeed,

    /// <summary>
    /// Разгон по значению температуры видеокарты.
    /// </summary>
    TargetTemperature,

    /// <summary>
    /// Графически-зависимый разгон.
    /// </summary>
    LinearDependence,
}
