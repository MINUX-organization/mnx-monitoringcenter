using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices;

/// <summary>
/// Динамические аппаратные показатели процессора.
/// </summary>
public class CpuDynamicHardwareIndicators : DeviceDynamicHardwareIndicators
{
    /// <inheritdoc/>
    public override DeviceType Type { get; } = DeviceType.CPU;

    /// <summary>
    /// Температура.
    /// </summary>
    public int Temperature { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }
}
