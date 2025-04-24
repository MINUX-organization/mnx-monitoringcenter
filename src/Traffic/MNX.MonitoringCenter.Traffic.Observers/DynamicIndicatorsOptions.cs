namespace MNX.MonitoringCenter.Traffic.Observers;

/// <summary>
/// Параметры динамических показателей.
/// </summary>
public class DynamicIndicatorsOptions
{
    /// <summary>
    /// Период обновления показателей в секундах.
    /// </summary>
    public int Interval { get; set; } = 2;

    /// <summary>
    /// Время ожидания.
    /// </summary>
    public int TimeOut { get; set; } = 10;
}
