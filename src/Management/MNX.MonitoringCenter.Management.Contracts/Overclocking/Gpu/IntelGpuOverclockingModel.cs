using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;

/// <summary>
/// Модель разгона для видеокарты Intel.
/// </summary>
public record IntelGpuOverclockingModel : IOverclockingModel
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
