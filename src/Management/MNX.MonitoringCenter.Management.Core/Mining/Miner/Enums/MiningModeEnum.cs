using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Mining.Miner.Enums;

/// <summary>
/// Режим майнинга монет.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MiningModeEnum
{
    /// <summary>
    /// Одна монета.
    /// </summary>
    Single = 1,

    /// <summary>
    /// Две монеты.
    /// </summary>
    Dual = 2,

    /// <summary>
    /// Три монеты.
    /// </summary>
    Triple = 3
}