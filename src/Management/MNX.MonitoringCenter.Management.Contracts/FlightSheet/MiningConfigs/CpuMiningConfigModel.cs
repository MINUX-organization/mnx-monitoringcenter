namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;

/// <summary>
/// Конфиг майнинга для процессора.
/// </summary>
public class CpuMiningConfigModel : BaseMiningConfigModel
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
