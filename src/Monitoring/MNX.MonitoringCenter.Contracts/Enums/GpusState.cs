using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Enums;

/// <summary>
/// Состояние видеокарты.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
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
