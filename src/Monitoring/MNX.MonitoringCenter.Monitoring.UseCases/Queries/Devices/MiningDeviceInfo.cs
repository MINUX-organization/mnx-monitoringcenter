namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.Devices;

/// <summary>
/// Информация о майнинг устройстве.
/// </summary>
public abstract class MiningDeviceInfo
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public string RigName { get; set; }

    /// <summary>
    /// Производитель.
    /// </summary>
    public string Manufacturer { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public string? FlightSheet { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string? Miner { get; set; }

    /// <summary>
    /// Серийный номер.
    /// </summary>
    public string SerialNumber { get; set; }
}
