using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

/// <summary>
/// Тип майнинг устройства.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MiningDeviceType
{
    /// <summary>
    /// Видеокарта.
    /// </summary>
    GPU,

    /// <summary>
    /// Процессор.
    /// </summary>
    CPU,

    /// <summary>
    /// Жёсткий диск.
    /// </summary>
    HDD
}
