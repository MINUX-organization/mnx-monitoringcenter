namespace MNX.MonitoringCenter.Monitoring.Contracts.Models;

/// <summary>
/// Модель параметра с единицей измерения.
/// </summary>
public class ParameterModelWithMeasureUnit
{
    /// <summary>
    /// Значение величины.
    /// </summary>
    public float Value { get; set; }

    /// <summary>
    /// Единица измерения.
    /// </summary>
    public string MeasureUnit { get; set; }
}
