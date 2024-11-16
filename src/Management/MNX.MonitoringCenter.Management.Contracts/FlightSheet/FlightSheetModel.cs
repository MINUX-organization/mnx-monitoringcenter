namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet;

/// <summary>
/// Модель полётного листа.
/// </summary>
public class FlightSheetModel
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
    public List<FlightSheetTargetModel> Targets { get; set; } = new(0);
}
