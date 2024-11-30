namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

/// <summary>
/// Группа объектов.
/// </summary>
public class Group<TElement>
{
    /// <summary>
    /// Название группы.
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Список элементов.
    /// </summary>
    public List<TElement> Elements { get; set; } = new(0);
}
