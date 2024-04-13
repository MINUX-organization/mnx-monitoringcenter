namespace MNX.MonitoringCenter.Monitoring.Service.Infrastructure;

/// <summary>
/// Параметры динамических данных.
/// </summary>
public class DynamicDataOptions
{
    /// <summary>
    /// Период обновления данных в секундах.
    /// </summary>
    public int UpdatePeriodInSeconds { get; set; }

    /// <summary>
    /// Количество точек данных в истории.
    /// </summary>
    public int PointCount { get; set; }
}
