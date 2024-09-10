using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet;

/// <summary>
/// Базовая модель полётного листа.
/// </summary>
[JsonDerivedType(typeof(CpuFlightSheetModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuFlightSheetModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuFlightSheetModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuFlightSheetModel), DiscriminatorValue = "GPU")]
public abstract class FlightSheetModelBase
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetConfigModel> Configs { get; set; } = new();

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public required string Miner { get; set; }
}
