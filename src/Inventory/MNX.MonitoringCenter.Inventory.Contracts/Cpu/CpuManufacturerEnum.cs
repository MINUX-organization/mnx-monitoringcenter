using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Производитель процессоров.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum CpuManufacturerEnum
{
    Amd,

    Intel
}