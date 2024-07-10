using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices.Cpu;

/// <summary>
/// Разгон процессора.
/// </summary>
public class CpuOverclocking : IOverclocking
{
    /// <summary>
    /// Размер страницы памяти.
    /// </summary>
    public int HugePages { get; set; }
}
