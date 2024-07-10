using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;

/// <summary>
/// Состояние видеокарты.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GpusState
{
    /// <summary>
    /// Карта активна (майнит)
    /// </summary>
    Active,

    /// <summary>
    /// Карта не активна (не майнит)
    /// </summary>
    Inactive,

    /// <summary>
    /// Ошибка
    /// </summary>
    Error,

    /// <summary>
    /// Пустой слот
    /// </summary>
    Empty
}
