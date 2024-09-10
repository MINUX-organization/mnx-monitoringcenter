namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet;

/// <summary>
/// Полётный лист для процессора.
/// </summary>
public class CpuFlightSheetModel : FlightSheetModelBase
{
    /// <summary>
    /// Страницы.
    /// </summary>
    public int HugePage { get; set; }

    /// <summary>
    /// Строка конфигурации формата Json.
    /// </summary>
    public string? ConfigFile { get; set; }
}
