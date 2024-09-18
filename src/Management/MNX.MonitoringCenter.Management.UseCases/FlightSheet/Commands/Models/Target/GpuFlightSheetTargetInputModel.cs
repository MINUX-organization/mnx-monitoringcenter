using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.Models.Target;

/// <summary>
/// Входная модель таргета полётного листа для видеокарты.
/// </summary>
public class GpuFlightSheetTargetInputModel : FlightSheetTargetInputModel
{
    /// <inheritdoc/>
    public override FlightSheetTargetType Type
    {
        get => FlightSheetTargetType.GPU;
    }
}
