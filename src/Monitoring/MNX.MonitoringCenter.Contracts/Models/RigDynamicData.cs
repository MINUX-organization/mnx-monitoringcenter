using MNX.MonitoringCenter.Monitoring.Contracts.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Динамические данные ригов.
/// </summary>
public class RigDynamicData : SystemData
{
    /// <summary>
    /// Индекс рига.
    /// </summary>
    public int Index { get; set; }


    /// <summary>
    /// Скорость интернета.
    /// </summary>
    public ParameterModelWithMeasureUnit InternetSpeed { get; set; }
}