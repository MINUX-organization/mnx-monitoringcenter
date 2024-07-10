using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Contracts.Bus.Enums;

/// <summary>
/// Уровень интернет соединения.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OnlineState
{
    Zero,
    One,
    Two,
    Three,
    Four
}