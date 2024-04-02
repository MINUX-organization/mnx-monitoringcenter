using MNX.MonitoringCenter.Monitoring.Contracts.Models;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Abstractions;

/// <summary>
/// Системные данные.
/// </summary>
public abstract class SystemData
{
    /// <summary>
    /// Униакльный идентификатор рига.
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
    public ParameterModelWithMeasureUnit Power { get; set; }

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
    public List<FlightSheetStatistics> FlightSheetInfo { get; set; } = new();
}
