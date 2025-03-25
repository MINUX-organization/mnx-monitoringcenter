using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Интерфейс модели разгона.
/// </summary>
[JsonDerivedType(typeof(GpuOverclockingModel), typeDiscriminator: "GPU")]
[JsonDerivedType(typeof(CpuOverclockingModel), typeDiscriminator: "CPU")]

[SwaggerSubType(typeof(GpuOverclockingModel), DiscriminatorValue = "GPU")]
[SwaggerSubType(typeof(CpuOverclockingModel), DiscriminatorValue = "CPU")]
public interface IOverclockingModel
{
    /// <summary>
    /// Тип целевого устройства.
    /// </summary>
    public OverclockingTargetDeviceType TargetDeviceType { get; }
}