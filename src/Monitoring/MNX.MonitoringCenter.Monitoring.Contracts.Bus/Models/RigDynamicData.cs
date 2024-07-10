using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Abstractions;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;

/// <summary>
/// Динамические данные ригов.
/// </summary>
public class RigDynamicData : IRig
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Средняя температура в градусах Цельсия.
    /// </summary>
    public int AverageTemperature { get; set; }

    /// <summary>
    /// Скорость вентилятора в процентах.
    /// </summary>
    public int FanSpeed { get; set; }

    /// <summary>
    /// Мощность.
    /// </summary>
    public int Power { get; set; }

    /// <summary>
    /// Скорость интернета.
    /// </summary>
    public int InternetSpeed { get; set; }

    /// <summary>
    /// Время майнинга.
    /// </summary>
    public DateTime MiningUpTime { get; set; }

    /// <summary>
    /// Время загрузки.
    /// </summary>
    public DateTime BootedUpTime { get; set; }

    /// <summary>
    /// Информация о полётных листах.
    /// </summary>
    public List<FlightSheetStatistics> FlightSheetsInfo { get; set; } = new();
}