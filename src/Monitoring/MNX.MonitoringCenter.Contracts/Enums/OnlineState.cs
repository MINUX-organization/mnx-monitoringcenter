using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Enums;

/// <summary>
/// Уровень интернет соединения.
/// </summary>
[JsonConverter(typeof(StringEnumConverter))]
public enum OnlineState
{
    Zero,
    One,
    Two,
    Three,
    Four
}