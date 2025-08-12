using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;

/// <summary>
/// Модель разгона для видеокарты Nvidia.
/// </summary>
public record NvidiaGpuOverclockingModel : IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.NvidiaGPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.NvidiaGPU;
    }

    /// <summary>
    /// Разгон вентилятора.
    /// </summary>
    public required IFanOverclockingModel FanOverclocking { get; set; }

    /// <summary>
    /// Ограничение мощности
    /// </summary>
    public int PowerLimit { get; set; }

    /// <summary>
    /// Фиксированная частота ядра
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Смещение частоты ядра
    /// </summary>
    public int CoreClockOffset { get; set; }

    /// <summary>
    /// Фиксированная частота памяти
    /// </summary>
    public int MemoryClockLock { get; set; }

    /// <summary>
    /// Смещение частоты памяти
    /// </summary>
    public int MemoryClockOffset { get; set; }

    /// <summary>
    /// Напряжение на ядре
    /// </summary>
    public int CoreVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения на ядре
    /// </summary>
    public int CoreVoltageOffset { get; set; }

    /// <summary>
    /// Напряжение памяти
    /// </summary>
    public int MemoryVoltage { get; set; }

    /// <summary>
    /// Смещение напряжения памяти
    /// </summary>
    public int MemoryVoltageOffset { get; set; }
}
