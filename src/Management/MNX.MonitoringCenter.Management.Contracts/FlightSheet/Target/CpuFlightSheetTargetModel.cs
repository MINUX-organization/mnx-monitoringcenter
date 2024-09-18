namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;

/// <summary>
/// Таргет полётного листа для процессора.
/// </summary>
public class CpuFlightSheetTargetModel : FlightSheetTargetModelBase
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
