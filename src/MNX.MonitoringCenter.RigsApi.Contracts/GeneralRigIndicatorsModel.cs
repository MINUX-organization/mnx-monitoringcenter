using MNX.MonitoringCenter.RigsApi.Contracts.FlightSheet;
using MNX.MonitoringCenter.Traffic.Contracts.Bus.Devices.Mining.FlightSheet;

namespace MNX.MonitoringCenter.RigsApi.Contracts;

public class GeneralRigIndicatorsModel : RigDynamicHardwareIndicatorsModel
{
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
    public List<RigCoinStatisticModel> TotalCoinStatistics { get; set; } = new();
}