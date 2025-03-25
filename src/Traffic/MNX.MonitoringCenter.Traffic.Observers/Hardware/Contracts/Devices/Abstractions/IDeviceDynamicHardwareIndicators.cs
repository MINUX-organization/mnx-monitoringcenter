using MNX.MonitoringCenter.Traffic.Observers.Abstractions;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

/// <summary>
/// Динамические аппаратные показатели устройства.
/// </summary>
public interface IDeviceDynamicHardwareIndicators : IDeviceIndicators
{
    /// <summary>
    /// Мощность.
    /// </summary>
    int Power { get; set; }
}
