using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices;

/// <summary>
/// Технология параллельных вычислений.
/// </summary>
public class ParallelComputingTechnology
{
    /// <summary>
    /// Тип.
    /// </summary>
    public ParallelComputingTechnologyEnum Type { get; set; }

    /// <summary>
    /// Версия.
    /// </summary>
    public string Version { get; set; }
}
