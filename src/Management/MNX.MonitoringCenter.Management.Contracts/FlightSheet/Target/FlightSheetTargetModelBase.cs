using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;

/// <summary>
/// Базовая модель таргета полётного листа.
/// </summary>
[JsonDerivedType(typeof(CpuFlightSheetTargetModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuFlightSheetTargetModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuFlightSheetTargetModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuFlightSheetTargetModel), DiscriminatorValue = "GPU")]
public abstract class FlightSheetTargetModelBase
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetTargetConfigModel> Configs { get; set; } = new();

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public required string MinerName { get; set; }
}
