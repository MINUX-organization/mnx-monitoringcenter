using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Минимальные и максимальные значения.
/// </summary>
[ComplexType]
public record RangeValue
{
    /// <summary>
    /// Минимальное значение.
    /// </summary>
    public int Minimal { get; init; }

    /// <summary>
    /// Максимальное значение.
    /// </summary>
    public int Maximal { get; init; }

    /// <summary>
    /// Можно ли изменять.
    /// </summary>
    public bool IsWritable { get; init; }

    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    public int Default { get; init; }
}
