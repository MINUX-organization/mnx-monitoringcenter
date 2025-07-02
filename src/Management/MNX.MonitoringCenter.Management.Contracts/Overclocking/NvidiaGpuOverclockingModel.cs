using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;
using MNX.MonitoringCenter.Management.Core.Overclocking;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Модель разгона для видеокарты Nvidia.
/// </summary>
public record NvidiaGpuOverclockingModel : NvidiaGpuOverclocking, IOverclockingModel
{
    private OverclockingTargetDeviceType _targetDeviceType = OverclockingTargetDeviceType.NvidiaGPU;

    /// <<inheritdoc/>
    [JsonPropertyName("$type")]
    public OverclockingTargetDeviceType TargetDeviceType
    {
        get => _targetDeviceType;
        init => _targetDeviceType = OverclockingTargetDeviceType.NvidiaGPU;
    }
}
