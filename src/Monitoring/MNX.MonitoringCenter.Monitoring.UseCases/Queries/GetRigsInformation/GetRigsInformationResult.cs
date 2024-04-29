namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Результат получения информации о ригах
/// </summary>
public class GetRigsInformationResult
{
    /// <summary>
    /// Риги.
    /// </summary>
    public IEnumerable<RigInformationMessage> Rigs { get; set; } = Enumerable.Empty<RigInformationMessage>();

    /// <summary>
    /// Общее кол-во ригов.
    /// </summary>
    public int TotalRigsCount
    {
        get => Rigs.Count();
    }

    /// <summary>
    /// Общее кол-во видеокарт в ригах по признакам.
    /// </summary>
    public TotalGpusCount TotalGpusCount { get; set; } = new();

    /// <summary>
    /// Общее кол-во процессоров в ригах по признакам.
    /// </summary>
    public TotalCpusCount TotalCpusCount { get; set; } = new();
}
