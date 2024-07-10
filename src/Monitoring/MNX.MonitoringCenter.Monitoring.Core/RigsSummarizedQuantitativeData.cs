namespace MNX.MonitoringCenter.Monitoring.Core;

/// <summary>
/// Обобщённые количественные данные ригов.
/// </summary>
public class RigsSummarizedQuantitativeData
{
    /// <summary>
    /// Общее кол-во ригов.
    /// </summary>
    public int TotalRigsCount { get; set; }

    /// <summary>
    /// Общее кол-во видеокарт в ригах по признакам.
    /// </summary>
    public TotalGpusCount TotalGpusCount { get; set; } = new();

    /// <summary>
    /// Общее кол-во процессоров в ригах по признакам.
    /// </summary>
    public TotalCpusCount TotalCpusCount { get; set; } = new();

    /// <summary>
    /// Общее кол-во жёстких дисков.
    /// </summary>
    public int TotalHddsCount { get; set; }
}
