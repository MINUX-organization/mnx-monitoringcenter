using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;

/// <summary>
/// Динамические показатели майнинга видеокарты.
/// </summary>
public class GpuDynamicMiningIndicators : DeviceDynamicMiningIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.GPU; }

    /// <summary>
    /// Температура памяти.
    /// </summary>
    public int MemoryTemperature { get; set; }

    /// <summary>
    /// Температура ядра.
    /// </summary>
    public int CoreTemperature { get; set; }

    /// <inheritdoc/>
    public override int GetAverageTemperature()
    {
        return (MemoryTemperature + CoreTemperature) / 2;
    }
}
