using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.RigsApi.Contracts.Device.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические общие показатели процессора.
/// </summary>
public class CpuDynamicTotalIndicators : DeviceDynamicTotalIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.CPU; }

    /// <summary>
    /// Температура.
    /// </summary>
    public int Temperature { get; set; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; set; }
}
