namespace MNX.MonitoringCenter.Traffic.Observers.Abstractions;

/// <summary>
/// Показатели рига.
/// </summary>
/// <typeparam name="TDeviceIndicators"></typeparam>
public interface IRigIndicators<TDeviceIndicators>
{
    /// <summary>
    /// Идентификатор рига.
    /// </summary>
    Guid RigId { get; init; }

    /// <summary>
    /// Устройства.
    /// </summary>
    List<TDeviceIndicators> Devices { get; init; }
}
