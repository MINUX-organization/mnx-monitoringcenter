using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

/// <summary>
/// Динамические параметры майнинга устройства.
/// </summary>
public abstract class DeviceDynamicMiningIndicators : IDeviceDynamicMiningIndicators
{
    /// <inheritdoc/>
    public Guid DeviceId { get; set; }

    /// <inheritdoc/>
    public abstract DeviceType Type { get; }

    /// <inheritdoc/>
    public FlightSheetStatistics? FlightSheet { get; set; }

    /// <inheritdoc/>
    public MiningState MiningState { get; init; }

    /// <summary>
    /// Время майнинга с момента последнего включения.
    /// </summary>
    public TimeOnly MiningUpTime { get; init; }
}
