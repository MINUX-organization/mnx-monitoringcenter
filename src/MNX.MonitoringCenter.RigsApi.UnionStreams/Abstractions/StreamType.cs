using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.RigsApi.UnionStreams.Abstractions;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum StreamType
{
    Monitoring,
    Devices,
    Rigs,
    Statistics,
}