namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;

/// <summary>
/// Динамические показатели устройства.
/// </summary>
public interface IDeviceDynamicIndicators
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    Guid DeviceId { get; init; }

    /// <summary>
    /// Тип устройства.
    /// </summary>
    DeviceType Type { get; }

    /// <summary>
    /// Мощность.
    /// </summary>
    int Power { get; init; }
}
