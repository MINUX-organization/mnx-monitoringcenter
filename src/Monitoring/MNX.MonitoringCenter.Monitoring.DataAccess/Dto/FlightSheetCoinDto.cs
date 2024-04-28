using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto;

/// <summary>
/// Dto полётного листа.
/// </summary>
public class FlightSheetCoinDto
{
    /// <summary>
    /// Уникальный идентификатор.
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// Идентификатор монеты.
    /// </summary>
    public Guid CoinId { get; set; }

    /// <summary>
    /// Монета.
    /// </summary>
    public Coin? Coin { get; set; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid FlightSheetId { get; set; }

    /// <summary>
    /// Полётный лист.
    /// </summary>
    public FlightSheetDto? FlightSheet { get; set; }

    /// <summary>
    /// Майнер.
    /// </summary>
    public string Miner { get; set; }
}
