using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Core.Overclocking.Enums;

/// <summary>
/// Тип целевого разгоняемого устройства.
/// </summary>
[JsonConverter(typeof(JsonStringEnumConverter))]
public enum OverclockingTargetDeviceType
{
    /// <summary>
    /// Процессор.
    /// </summary>
    CPU,

    /// <summary>
    /// Видеокарта AMD.
    /// </summary>
    AmdGPU,

    /// <summary>
    /// Видеокарта Nvidia.
    /// </summary>
    NvidiaGPU,

    /// <summary>
    /// Видеокарта Intel.
    /// </summary>
    IntelGPU,
}
