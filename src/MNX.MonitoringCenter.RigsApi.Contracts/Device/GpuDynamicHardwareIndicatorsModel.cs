using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Device;

/// <summary>
/// Динамические аппаратные показатели видеокарты.
/// </summary>
public class GpuDynamicHardwareIndicatorsModel : DeviceDynamicHardwareIndicatorsModel
{
    /// <inheritdoc/>
    public override DeviceType Type { get => DeviceType.GPU; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }

    /// <summary>
    /// Средняя температура.
    /// </summary>
    public int AvarageTemperature { get; init; }
}
