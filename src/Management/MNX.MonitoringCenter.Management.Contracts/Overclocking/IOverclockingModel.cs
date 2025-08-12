using MNX.MonitoringCenter.Management.Contracts.Overclocking.Cpu;
using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

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