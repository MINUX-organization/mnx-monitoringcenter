using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts.Rigs;

public class MiningRigIndicatorsModel
{
    /// <summary>
    /// Уникальный идентификатор рига.
    /// </summary>
    public Guid RigId { get; set; }

    /// <summary>
    /// Название рига.
    /// </summary>
    public required string RigName { get; set; }

    /// <summary>
    /// Время майнинга с момента последнего включения.
    /// </summary>
    public DateTime MiningUpTime { get; set; }

    /// <summary>
    /// Общее кол-во решений.
    /// </summary>
    public required SharesModel TotalShares { get; set; }

    /// <summary>
    /// Общая скорость хеширования.
    /// </summary>
    public int TotalHashRate { get; set; }

    /// <summary>
    /// Обобщённая статистика по монетам.
    /// </summary>
    public List<RigCoinStatistics> TotalCoinStatistics { get; set; } = new();
}