using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Information;

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
