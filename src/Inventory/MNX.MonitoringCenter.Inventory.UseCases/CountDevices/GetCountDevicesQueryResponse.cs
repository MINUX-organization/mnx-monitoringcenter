namespace MNX.MonitoringCenter.Inventory.UseCases.CountDevices;

/// <summary>
/// Ответ на <see cref="GetCountDevicesQueryResponse"/>.
/// </summary>
public class GetCountDevicesQueryResponse
{
    /// <summary>
    /// Общее кол-во процессоров, сгруппированных по производителю.
    /// </summary>
    public Dictionary<string, int> TotalCpusCountGroupedByManufacturer { get; init; } = new(0);

    /// <summary>
    /// Общее кол-во процессоров.
    /// </summary>
    public int TotalCpusCount { get; init; }

    /// <summary>
    /// Общее кол-во видеокарт, сгруппированных по производителю.
    /// </summary>
    public Dictionary<string, int> TotalGpusCountGroupedByManufacturer { get; init; } = new(0);

    /// <summary>
    /// Общее кол-во видеокарт.
    /// </summary>
    public int TotalGpusCount { get; init; }

    /// <summary>
    /// Общее кол-во жёстких дисков.
    /// </summary>
    public int TotalDrivesCount { get; init; }
}
