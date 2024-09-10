namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

/// <summary>
/// Входная модель с конфигом для полётного листа.
/// </summary>
public class FlightSheetConfigInputModel
{
    /// <summary>
    /// Идентификатор пула.
    /// </summary>
    public Guid PoolId { get; set; }

    /// <summary>
    /// Идентификатор кошелька.
    /// </summary>
    public Guid WalletId { get; set; }
}
