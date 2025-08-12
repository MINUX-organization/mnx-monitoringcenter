using MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu.Fan;
using MNX.MonitoringCenter.Management.Core.Overclocking.Enums;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.Overclocking.Gpu;

/// <summary>
/// Модель разгона вентилятора вентилятора.
/// </summary>
[JsonDerivedType(typeof(FanOverclockingWithTargetSpeedModel), typeDiscriminator: "TargetSpeed")]
[JsonDerivedType(typeof(FanOverclockingWithTargetTemperatureModel), typeDiscriminator: "TargetTemperature")]
[JsonDerivedType(typeof(FanOverclockingWithLinearDependenceModel), typeDiscriminator: "LinearDependence")]

[SwaggerSubType(typeof(FanOverclockingWithTargetSpeedModel), DiscriminatorValue = "TargetSpeed")]
[SwaggerSubType(typeof(FanOverclockingWithTargetTemperatureModel), DiscriminatorValue = "TargetTemperature")]
[SwaggerSubType(typeof(FanOverclockingWithLinearDependenceModel), DiscriminatorValue = "LinearDependence")]
public interface IFanOverclockingModel
{
    /// <summary>
    /// Тип разгона вентилятора.
    /// </summary>
    public FanOverclockingType FanOverclockingType { get; }
}
