namespace MNX.MonitoringCenter.Monitoring.UseCases.Commands.ComputeTotalRigsDynamicData.Models;

/// <summary>
/// Модель скорости хеширования.
/// </summary>
public class HashRateModel
{
    /// <summary>
    /// Время.
    /// </summary>
    public DateTimeOffset Time { get; set; }

    /// <summary>
    /// Значение.
    /// </summary>
    public ParameterModelWithMeasureUnit Value { get; set; }
}
