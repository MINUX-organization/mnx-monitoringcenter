namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient.Models;

/// <summary>
/// Общее кол-во процессоров по признакам.
/// </summary>
public class TotalCpusCount
{
    /// <summary>
    /// Общее кол-во.
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// Кол-во процессоров Amd.
    /// </summary>
    public int Amd { get; set; }
    /// <summary>
    /// Кол-во процессоров Intel.
    /// </summary>
    public int Intel { get; set; }
}
