using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;
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

    /// <summary>
    /// Адрес локального рига.
    /// </summary>
    public string? LocalIp { get; set; }

    /// <summary>
    /// Версия Minux.
    /// </summary>
    public string? MinuxVersion { get; set; }

    /// <summary>
    /// Кол-во устройств.
    /// </summary>
    public required ModelWithCountDevices? CountDevices { get; init; }
}