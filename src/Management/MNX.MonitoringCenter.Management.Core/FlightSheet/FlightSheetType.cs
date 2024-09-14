using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.FlightSheet;

/// <summary>
/// Тип полётного листа.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FlightSheetType
{
    /// <summary>
    /// Для процессора.
    /// </summary>
    CPU,

    /// <summary>
    /// Для видеокарты.
    /// </summary>
    GPU
}
