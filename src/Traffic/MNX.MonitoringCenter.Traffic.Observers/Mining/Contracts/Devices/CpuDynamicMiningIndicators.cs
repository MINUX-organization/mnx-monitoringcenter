using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices;

/// <summary>
/// Динамические показатели майнинга процессора.
/// </summary>
public class CpuDynamicMiningIndicators : DeviceDynamicMiningIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.CPU; }
}
