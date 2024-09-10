using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

/// <summary>
/// Входная модель полётного листа для видеокарты.
/// </summary>
public class GpuFlightSheetInputModel : FlightSheetInputModel
{
    /// <inheritdoc/>
    public override FlightSheetType Type
    {
        get => FlightSheetType.GPU;
    }
}
