using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для видеокарты.
/// </summary>
public record GpuOverclockingModel : Inventory.Contracts.Devices.Gpu.GpuOverclocking, IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.GPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.GPU;
    }
}
