using MNX.MonitoringCenter.Management.Core.Overclocking;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для процессора.
/// </summary>
public record CpuOverclockingModel : Inventory.Contracts.Devices.Cpu.CpuOverclocking, IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.CPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.CPU;
    }
}