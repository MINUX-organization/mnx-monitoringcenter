using System.Text.Json.Serialization;
using Swashbuckle.AspNetCore.Annotations;
using MNX.MonitoringCenter.Management.Core.Overclocking;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Интерфейс модели разгона.
/// </summary>
[JsonDerivedType(typeof(AmdGpuOverclockingModel), typeDiscriminator: "AmdGPU")]
[JsonDerivedType(typeof(NvidiaGpuOverclockingModel), typeDiscriminator: "NvidiaGPU")]
[JsonDerivedType(typeof(IntelGpuOverclockingModel), typeDiscriminator: "IntelGPU")]
[JsonDerivedType(typeof(CpuOverclockingModel), typeDiscriminator: "CPU")]

[SwaggerSubType(typeof(AmdGpuOverclockingModel), DiscriminatorValue = "AmdGPU")]
[SwaggerSubType(typeof(NvidiaGpuOverclockingModel), DiscriminatorValue = "NvidiaGPU")]
[SwaggerSubType(typeof(IntelGpuOverclockingModel), DiscriminatorValue = "IntelGPU")]
[SwaggerSubType(typeof(CpuOverclockingModel), DiscriminatorValue = "CPU")]
public interface IOverclockingModel
{
    /// <summary>
    /// Тип целевого устройства.
    /// </summary>
    public OverclockingTargetDeviceType TargetDeviceType { get; }
}