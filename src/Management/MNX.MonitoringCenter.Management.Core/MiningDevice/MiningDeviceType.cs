using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.MiningDevice;

/// <summary>
/// Тип майнинг устройства.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MiningDeviceType
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
