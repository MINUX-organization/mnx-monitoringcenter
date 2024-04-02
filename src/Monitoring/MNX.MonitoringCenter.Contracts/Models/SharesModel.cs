namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Модель с Shares
/// </summary>
public class SharesModel
{
    /// <summary>
    /// Принятые решения.
    /// </summary>
    public int Accepted { get; set; }

    /// <summary>
    /// Отклонённые решения.
    /// </summary>
    public int Rejected { get; set; }
}
