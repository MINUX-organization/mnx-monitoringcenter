namespace MNX.MonitoringCenter.Monitoring.UseCases.Queries.GetRigsInformation;

/// <summary>
/// Модель майнинг устройства.
/// </summary>
public class MiningDeviceModel
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheetModel FlightSheet { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }
}
