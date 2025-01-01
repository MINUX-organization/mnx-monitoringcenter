using MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические показатели майнинга процессора.
/// </summary>
public class CpuDynamicMiningIndicatorsModel : DeviceDynamicMiningIndicatorsModel
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.CPU; }
}
