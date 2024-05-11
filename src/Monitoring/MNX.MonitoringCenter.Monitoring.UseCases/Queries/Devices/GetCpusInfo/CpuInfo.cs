namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices.GetCpusInfo;

/// <summary>
/// Информация о процессоре.
/// </summary>
public class CpuInfo : MiningDeviceInfo
{
    /// <summary>
    /// Архитектура.
    /// </summary>
    public string Architecture { get; set; }

    /// <summary>
    /// Количество ядер.
    /// </summary>
    public int CoresCount { get; set; }

    /// <summary>
    /// Количество потоков.
    /// </summary>
    public int ThreadsCount { get; set; }

    /// <summary>
    /// Количество потоков на сокет.
    /// </summary>
    public int ThreadsPerSocketCount { get; set; }

    /// <summary>
    /// Минимальная частота ядра.
    /// </summary>
    public float MinClock { get; set; }

    /// <summary>
    /// Максимальная частота ядра.
    /// </summary>
    public float MaxClock { get; set; }
}
