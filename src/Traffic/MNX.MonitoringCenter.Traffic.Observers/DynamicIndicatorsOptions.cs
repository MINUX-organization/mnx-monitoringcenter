namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Параметры динамических показателей.
/// </summary>
public class DynamicIndicatorsOptions
{
    /// <summary>
    /// Период обновления показателей в секундах.
    /// </summary>
    public int UpdatePeriodInSeconds { get; set; } = 2;
}
