namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;

/// <summary>
/// Динамические показатели устройства.
/// </summary>
public abstract class DeviceDynamicIndicators : IDeviceDynamicIndicators
{
    /// <inheritdoc/>
    public Guid DeviceId { get; init; }

    /// <inheritdoc/>
    public abstract DeviceType Type { get; }

    /// <summary>
    /// Мощность.
    /// </summary>
    public int Power { get; init; }
}
