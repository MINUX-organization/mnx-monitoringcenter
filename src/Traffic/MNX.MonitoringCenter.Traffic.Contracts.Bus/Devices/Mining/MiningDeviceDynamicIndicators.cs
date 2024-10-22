using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

/// <summary>
/// Динамические показатели майнинг устройства.
/// </summary>
public abstract class MiningDeviceDynamicIndicators : DeviceDynamicIndicators
{
    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheetStatistics? FlightSheet { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; init; }

    /// <summary>
    /// Состояние майнинга.
    /// </summary>
    public MiningState MiningState { get; init; }

    /// <summary>
    /// Получить среднюю температуру.
    /// </summary>
    /// <returns> Средняя температура. </returns>
    public abstract int GetAverageTemperature();
}
