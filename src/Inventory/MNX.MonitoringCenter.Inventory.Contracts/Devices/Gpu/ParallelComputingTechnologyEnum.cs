using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;

/// <summary>
/// Технология параллельных вычислений.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ParallelComputingTechnologyEnum
{
    CUDA,

    OpenCL
}
