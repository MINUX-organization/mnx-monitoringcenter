using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.MiningConfig;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models;

/// <summary>
/// Входная модель таргерта полётного листа.
/// </summary>
public class FlightSheetTargetInputModel
{
    /// <summary>
    /// Конфиг для майнинга.
    /// </summary>
    public required MiningConfigInputModel MiningConfig { get; init; }

    /// <summary>
    /// Идентификатор майнера.
    /// </summary>
    public Guid MinerId { get; init; }
}
