using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Enums;

/// <summary>
/// Режим майнинга монет на GPU.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GpuMiningModeEnum
{
    Single = 1, 

    Dual = 2,

    Triple = 3
}