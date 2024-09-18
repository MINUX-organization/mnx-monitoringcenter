using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

/// <summary>
/// Тип таргета полётного листа.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum FlightSheetTargetType
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
