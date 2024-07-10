using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Messages.Models;

/// <summary>
/// Монета полётного листа.
/// </summary>
public class FlightSheetCoin
{
    /// <summary>
    /// Монета.
    /// </summary>
    public string Coin { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public string FlightSheet { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }

    /// <summary>
    /// Скорость хеширования.
    /// </summary>
    public ParameterModelWithMeasureUnit HashRate { get; set; }

    /// <summary>
    /// Шеры.
    /// </summary>
    public SharesModel Shares { get; set; }
}
