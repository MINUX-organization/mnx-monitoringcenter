using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Monitoring.Core.Devices.Enums;

/// <summary>
/// Производитель видеокарт.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum GpuManufacturerEnum
{
    Nvidia,

    Amd,

    Intel
}

/// <summary>
/// Производитель процессоров.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CpuManufacturerEnum
{
    Amd,

    Intel
}
