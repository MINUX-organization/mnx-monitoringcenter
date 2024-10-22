using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using MNX.MonitoringCenter.Management.Core.Miner;

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
    /// Майнер.
    /// </summary>
    public Miner Miner { get; set; }
}
