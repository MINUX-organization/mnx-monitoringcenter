using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;

namespace MNX.MonitoringCenter.Traffic.Observers.Abstractions;

/// <summary>
/// Показатели устройств.
/// </summary>
public interface IDeviceIndicators
{
    /// <summary>
    /// Идентификатор устройства.
    /// </summary>
    Guid DeviceId { get; init; }

    /// <summary>
    /// Тип устройства.
    /// </summary>
    DeviceType Type { get; }
}
