using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking;

/// <summary>
/// Интерфейс модели разгона.
/// </summary>
[JsonDerivedType(typeof(GpuOverclockingModel), typeDiscriminator: "GPU")]
[SwaggerSubType(typeof(GpuOverclockingModel), DiscriminatorValue = "GPU")]
public interface IOverclockingModel { }