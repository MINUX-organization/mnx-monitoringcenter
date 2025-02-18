namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.Models;

/// <summary>
/// Входная модель полётного листа.
/// </summary>
public class FlightSheetInputModel
{
    /// <summary>
    /// Название.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    /// Таргеты.
    /// </summary>
    public List<FlightSheetTargetInputModel> Targets { get; init; } = new(2);
}
