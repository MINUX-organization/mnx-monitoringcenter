using MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries.Models;

/// <summary>
/// Выходная модель таргета полетного листа.
/// </summary>
public class FlightSheetTargetOutputModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; init; }

    /// <summary>
    /// Список конфигов для майнинга.
    /// </summary>
    public List<FlightSheetTargetConfigModel> Configs { get; set; } = [];

    /// <summary>
    /// Строка аргументов для майнера.
    /// </summary>
    public string? AdditionalArguments { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public required Core.Miner Miner { get; set; }
}