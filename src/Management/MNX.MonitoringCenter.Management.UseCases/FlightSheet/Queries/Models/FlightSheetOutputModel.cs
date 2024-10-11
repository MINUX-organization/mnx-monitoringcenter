namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries.Models;

/// <summary>
/// Выходная модель полетного листа.
/// </summary>
public class FlightSheetOutputModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public required Guid Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Таргеты.
    /// </summary>
    public List<FlightSheetTargetOutputModel> Targets { get; set; } = new(2);
}