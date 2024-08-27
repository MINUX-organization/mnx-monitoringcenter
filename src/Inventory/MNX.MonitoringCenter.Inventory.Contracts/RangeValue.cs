using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts;

/// <summary>
/// Минимальные и максимальные значения.
/// </summary>
[ComplexType]
public sealed record RangeValue
{
    /// <summary>
    /// Минимальное значение.
    /// </summary>
    public int Minimal { get; set; }

    /// <summary>
    /// Максимальное значение.
    /// </summary>
    public int Maximal { get; set; }

    /// <summary>
    /// Можно ли изменять.
    /// </summary>
    public bool IsWritable { get; set; }

    /// <summary>
    /// Значение по умолчанию.
    /// </summary>
    public int Default { get; set; }
}
