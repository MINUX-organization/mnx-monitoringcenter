using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu;

/// <summary>
/// Поддерживаемые производители видеокарт.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum SupportedGpuManufacturer
{
    AMD,

    Intel,

    Nvidia
}
