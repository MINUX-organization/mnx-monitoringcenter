using MNX.MonitoringCenter.Management.Core.FlightSheet;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

/// <summary>
/// Базовая входная модель полётного листа.
/// </summary>
[JsonDerivedType(typeof(CpuFlightSheetInputModel), typeDiscriminator: "CPU")]
[JsonDerivedType(typeof(GpuFlightSheetInputModel), typeDiscriminator: "GPU")]

[SwaggerSubType(typeof(CpuFlightSheetInputModel), DiscriminatorValue = "CPU")]
[SwaggerSubType(typeof(GpuFlightSheetInputModel), DiscriminatorValue = "GPU")]
public abstract class FlightSheetInputModel
{
    /// <summary>
    /// Тип.
    /// </summary>
    public abstract FlightSheetType Type { get; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetConfigInputModel> Configs { get; set; } = new();

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Название майнера.
    /// </summary>
    public string Miner { get; set; } = string.Empty;
}
