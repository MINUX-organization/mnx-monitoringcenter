using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;

/// <summary>
/// Тип майнера.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MinerTypeEnum
{
    Integrated,
    Custom
}