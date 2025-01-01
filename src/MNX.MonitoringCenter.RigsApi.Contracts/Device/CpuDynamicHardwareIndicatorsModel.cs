using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические аппаратные показатели процессора.
/// </summary>
public class CpuDynamicHardwareIndicatorsModel : DeviceDynamicHardwareIndicatorsModel
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.CPU; }

    /// <summary>
    /// Температура.
    /// </summary>
    public int Temperature { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }
}
