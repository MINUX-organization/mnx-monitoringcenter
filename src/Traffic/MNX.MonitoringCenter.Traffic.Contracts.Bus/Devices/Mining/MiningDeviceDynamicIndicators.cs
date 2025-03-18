using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Abstractions;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

/// <summary>
/// Динамические показатели майнинг устройства.
/// </summary>
public abstract class MiningDeviceDynamicIndicators : DeviceDynamicIndicators
{
    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int FanSpeed { get; set; }

    /// <summary>
    /// Состояние майнинга.
    /// </summary>
    public MiningState MiningState { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string? MinerName { get; set; }

    /// <summary>
    /// Метрики майнинга монет.
    /// </summary>
    public List<MiningMetrics> Coins { get; set; } = new(0);

    /// <summary>
    /// Время майнинга в секундах с момента последнего включения.
    /// </summary>
    public int MiningUpTimeInSeconds { get; set; }

    /// <summary>
    /// Получить среднюю температуру.
    /// </summary>
    /// <returns> Средняя температура. </returns>
    public abstract int GetAverageTemperature();
}
