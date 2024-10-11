using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;

namespace MNX.MonitoringCenter.Traffic.Observers.Hardware.Contracts.Devices.Abstractions;

/// <summary>
/// Динамические аппаратные показатели устройства.
/// </summary>
public abstract class DeviceDynamicHardwareIndicators : IDeviceDynamicHardwareIndicators
{
    /// <inheritdoc/>
    public Guid DeviceId { get; init; }

    /// <inheritdoc/>
    public abstract DeviceType Type { get; }

    /// <inheritdoc/>
    public int Power { get; init; }
}
