using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Network;

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