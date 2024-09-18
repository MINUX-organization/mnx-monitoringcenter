using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

/// <summary>
/// Базовая входная модель таргерта полётного листа.
/// </summary>
[JsonDerivedType(typeof(CpuFlightSheetTargetInputModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuFlightSheetTargetInputModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuFlightSheetTargetInputModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuFlightSheetTargetInputModel), DiscriminatorValue = "GPU")]
public abstract class FlightSheetTargetInputModel
{
    /// <summary>
    /// Тип.
    /// </summary>
    public abstract FlightSheetTargetType Type { get; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetTargetConfigInputModel> Configs { get; init; } = new();

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; init; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; init; }
}
