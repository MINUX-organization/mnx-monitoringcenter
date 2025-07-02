using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для видеокарты Intel.
/// </summary>
public record IntelGpuOverclockingModel : IntelGpuOverclocking, IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.IntelGPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.IntelGPU;
    }
}
