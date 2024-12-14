using MNX.MonitoringCenter.Management.Core.Overclocking;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для процессора.
/// </summary>
public record CpuOverclockingModel : Inventory.Contracts.Devices.Cpu.CpuOverclocking, IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init
        {
            if (value != OverclockingTargetDeviceType.CPU)
                throw new ArgumentException("Target device type is not supported", nameof(TargetDeviceType));

            _targetDeviceType = value;
        }
    }
}