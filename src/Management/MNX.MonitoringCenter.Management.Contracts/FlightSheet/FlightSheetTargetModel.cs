using MNX.MonitoringCenter.Management.Contracts.FlightSheet.MiningConfigs;
using MNX.MonitoringCenter.Management.Core.Miner;

namespace MNX.MonitoringCenter.Management.Contracts.FlightSheet;

/// <summary>
/// Модуль таргета полётного листа.
/// </summary>
public class FlightSheetTargetModel
{
    /// <summary>
    /// Конфиг для майнинга.
    /// </summary>
    public required BaseMiningConfigModel MiningConfig { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public required Miner Miner { get; set; }
}
