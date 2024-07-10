using MNX.MonitoringCenter.Monitoring.Core.Devices.Abstractions;
using MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices.Cpu;

/// <summary>
/// Процессор.
/// </summary>
public class Cpu : MiningDevice
{
    /// <inheritdoc/>
    public override MiningDeviceType Type
    {
        get => MiningDeviceType.CPU;
    }

    /// <summary>
    /// Архитектура.
    /// </summary>
    public string Architecture { get; }

    /// <summary>
    /// Количество ядер.
    /// </summary>
    public int CoresCount { get; }

    /// <summary>
    /// Количество потоков.
    /// </summary>
    public int ThreadsCount { get; }

    /// <summary>
    /// Количество потоков на сокет.
    /// </summary>
    public int ThreadsPerSocketCount { get; }

    /// <summary>
    /// Минимальная частота ядра.
    /// </summary>
    public float MinClock { get; }

    /// <summary>
    /// Максимальная частота ядра.
    /// </summary>
    public float MaxClock { get; }

    public Cpu(Guid id, string name, string rigName, CpuManufacturerEnum manufacturer, string serialNumber,
               string architecture, int coresCount, int threadsCount, int threadsPerSocketCount,
               float minClock, float maxClock)
        : base(id, name, rigName, manufacturer.ToString(), serialNumber)
    {
        Architecture = architecture;
        CoresCount = coresCount;
        ThreadsCount = threadsCount;
        ThreadsPerSocketCount = threadsPerSocketCount;
        MinClock = minClock;
        MaxClock = maxClock;
    }
}
