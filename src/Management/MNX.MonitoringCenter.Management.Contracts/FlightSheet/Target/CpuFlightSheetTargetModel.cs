namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.Target;

/// <summary>
/// Таргет полётного листа для процессора.
/// </summary>
public class CpuFlightSheetTargetModel : FlightSheetTargetModelBase
{
    /// <summary>
    /// Страницы.
    /// </summary>
    public int? HugePages { get; set; }

    /// <summary>
    /// Кол-во потоков.
    /// </summary>
    public int? ThreadsCount { get; set; }
}
