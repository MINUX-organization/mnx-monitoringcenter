using MNX.MonitoringCenter.Traffic.Observers.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;
using MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.FlightSheet;

namespace MNX.MonitoringCenter.Traffic.Observers.Mining.Contracts.Devices.Abstractions;

/// <summary>
/// Динамические параметры майнинга устройства.
/// </summary>
public interface IDeviceDynamicMiningIndicators : IDeviceIndicators
{
    /// <summary>
    /// Статистика полётного листа.
    /// </summary>
    public FlightSheetStatistics? FlightSheet { get; set; }

    /// <summary>
    /// Состояние майнинга.
    /// </summary>
    public MiningState MiningState { get; init; }

    /// <summary>
    /// Время майнинга в секундах с момента последнего включения.
    /// </summary>
    public int MiningUpTimeInSeconds { get; init; }
}
