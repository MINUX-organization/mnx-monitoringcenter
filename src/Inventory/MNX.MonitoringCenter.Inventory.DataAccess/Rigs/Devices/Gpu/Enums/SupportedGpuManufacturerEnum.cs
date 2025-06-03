using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Enums;

/// <summary>
/// Поддерживаемые производители видеокарт.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SupportedGpuManufacturerEnum
{
    AMD,

    Intel,

    Nvidia
}
