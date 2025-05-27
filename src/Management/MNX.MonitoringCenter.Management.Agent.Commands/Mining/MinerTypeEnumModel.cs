using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Agent.Commands.Mining;

/// <summary>
/// Модель перечисления типа майнера.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MinerTypeEnumModel
{
    Integrated,
    Custom
}
