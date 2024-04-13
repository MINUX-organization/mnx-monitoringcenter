using MNX.MonitoringCenter.Monitoring.Contracts.Bus.Models;
using MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;

namespace MNX.MonitoringCenter.Monitoring.Service.Messages.Models;

/// <summary>
/// Модель полётного листа.
/// </summary>
public class FlightSheetModel
{
    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Монета.
    /// </summary>
    public string Coin { get; set; }

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
