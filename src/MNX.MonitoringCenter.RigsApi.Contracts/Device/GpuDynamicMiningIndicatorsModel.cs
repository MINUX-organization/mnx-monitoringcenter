using MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические показатели майнинга видеокарты.
/// </summary>
public class GpuDynamicMiningIndicatorsModel : DeviceDynamicMiningIndicatorsModel
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.GPU; }
}
