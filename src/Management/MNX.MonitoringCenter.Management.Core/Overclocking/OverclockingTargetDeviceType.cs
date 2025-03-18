using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Overclocking;

/// <summary>
/// Тип целевого разгоняемого устройства.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OverclockingTargetDeviceType
{
    /// <summary>
    /// Процессор.
    /// </summary>
    CPU,

    /// <summary>
    /// Видеокарта.
    /// </summary>
    GPU
}
