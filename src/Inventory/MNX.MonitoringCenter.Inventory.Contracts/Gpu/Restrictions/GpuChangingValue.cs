using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Изменяемые значения видеокарты.
/// </summary>
[ComplexType]
public sealed record GpuChangingValue
{
    /// <summary>
    /// Закрытие ядра.
    /// </summary>
    public RangeValue Lock { get; set; }

    /// <summary>
    /// Сдвиг ядра.
    /// </summary>
    public RangeValue Offset { get; set; }
}
