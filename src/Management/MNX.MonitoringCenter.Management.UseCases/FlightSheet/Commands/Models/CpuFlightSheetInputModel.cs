using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.Models;

/// <summary>
/// Входная модель полётного листа для процессора.
/// </summary>
public class CpuFlightSheetInputModel : FlightSheetInputModel
{
    /// <inheritdoc/>
    public override FlightSheetType Type
    {
        get => FlightSheetType.CPU;
    }

    /// <summary>
    /// Страницы.
    /// </summary>
    public int HugePage { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFile { get; set; }
}
