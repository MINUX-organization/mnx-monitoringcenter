using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;

/// <summary>
/// Тип наблюдаемых объектов.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ObservableObjectsType
{
    /// <summary>
    /// Риги.
    /// </summary>
    Rigs,

    /// <summary>
    /// Устройства.
    /// </summary>
    Devices
}
