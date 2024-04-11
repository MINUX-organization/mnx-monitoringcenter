using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Service.Messages.Models;

/// <summary>
/// Тип объектов, для которых высчитываются обобщённые данные.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum TotalDataType
{
    /// <summary>
    /// Шеры.
    /// </summary>
    TotalShares,
    /// <summary>
    /// Мощность.
    /// </summary>
    TotalPower,
    /// <summary>
    /// Кол-во ригов.
    /// </summary>
    TotalRigsCount,
    /// <summary>
    /// Кол-во видеокарт по признакам.
    /// </summary>
    TotalGpusCount,
    /// <summary>
    /// Кол-во процессоров по признакам.
    /// </summary>
    TotalCpusCount,
    /// <summary>
    /// Обобщённая статистика по монетам.
    /// </summary>
    TotalCoinsList
}
