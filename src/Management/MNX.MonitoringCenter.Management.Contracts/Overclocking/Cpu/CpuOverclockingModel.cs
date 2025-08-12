using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;

/// <summary>
/// Модель разгона для процессора.
/// </summary>
public record CpuOverclockingModel : IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.CPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.CPU;
    }

    /// <summary>
    /// Фиксированная частота ядра.
    /// </summary>
    public int CoreClockLock { get; set; }

    /// <summary>
    /// Напряжение.
    /// </summary>
    public int CoreVoltage { get; set; }
}