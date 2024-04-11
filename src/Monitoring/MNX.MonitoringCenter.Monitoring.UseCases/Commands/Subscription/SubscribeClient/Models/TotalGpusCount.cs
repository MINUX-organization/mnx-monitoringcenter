namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.Subscription.SubscribeClient.Models;

/// <summary>
/// Общее кол-во видеокарт по признакам.
/// </summary>
public class TotalGpusCount
{
    /// <summary>
    /// Общее кол-во.
    /// </summary>
    public int Total { get; set; }
    /// <summary>
    /// Кол-во карт Amd.
    /// </summary>
    public int Amd { get; set; }
    /// <summary>
    /// Кол-во карт Nvidia.
    /// </summary>
    public int Nvidia { get; set; }
    /// <summary>
    /// Кол-во карт Intel.
    /// </summary>
    public int Intel { get; set; }
}
