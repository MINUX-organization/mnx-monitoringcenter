namespace MNX.MonitoringCenter.Monitoring.DataAccess.Dto;

/// <summary>
/// Dto полётного листа.
/// </summary>
public class FlightSheetDto
{
    /// <summary>
    /// Идентификатор.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Список монет.
    /// </summary>
    public List<FlightSheetCoinDto> Coins { get; set; } = new();
}
