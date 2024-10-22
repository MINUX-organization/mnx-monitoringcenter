using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining;

/// <summary>
/// Состояние майнинга.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MiningState
{
    /// <summary>
    /// Майнит.
    /// </summary>
    Active,

    /// <summary>
    /// Не майнит.
    /// </summary>
    Inactive,

    /// <summary>
    /// Ошибка.
    /// </summary>
    Error
}
