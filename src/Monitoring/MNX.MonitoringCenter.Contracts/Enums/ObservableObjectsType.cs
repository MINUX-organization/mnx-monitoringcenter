using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Enums;

/// <summary>
/// Тип наблюдаемых объектов.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum ObservableObjectsType
{
    /// <summary>
    /// Риги.
    /// </summary>
    Rigs,

    /// <summary>
    /// Устройства.
    /// </summary>
    Devices,

    /// <summary>
    /// Полётные листы.
    /// </summary>
    FlightSheets
}
