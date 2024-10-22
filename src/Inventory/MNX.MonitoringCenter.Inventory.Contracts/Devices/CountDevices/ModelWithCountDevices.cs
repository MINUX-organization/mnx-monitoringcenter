namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;

/// <summary>
/// Ответ на <see cref="ModelWithCountDevices"/>.
/// </summary>
public class ModelWithCountDevices
{
    /// <summary>
    /// Общее кол-во процессоров, сгруппированных по производителю.
    /// </summary>
    public Dictionary<string, int> TotalCpusCountGroupedByManufacturer { get; init; } = new(0);

    /// <summary>
    /// Общее кол-во процессоров.
    /// </summary>
    private int? _totalCpusCount;
    public int TotalCpusCount
    {
        get => _totalCpusCount ??= TotalCpusCountGroupedByManufacturer.Sum(x => x.Value);
        init => _totalCpusCount = value;
    }

    /// <summary>
    /// Общее кол-во видеокарт, сгруппированных по производителю.
    /// </summary>
    public Dictionary<string, int> TotalGpusCountGroupedByManufacturer { get; init; } = new(0);

    /// <summary>
    /// Общее кол-во видеокарт.
    /// </summary>
    private int? _totalGpusCount;
    public int TotalGpusCount
    {
        get => _totalGpusCount ??= TotalGpusCountGroupedByManufacturer.Sum(x => x.Value);
        init => _totalGpusCount = value;
    }

    /// <summary>
    /// Общее кол-во жёстких дисков.
    /// </summary>
    public int TotalDrivesCount { get; init; }
}
