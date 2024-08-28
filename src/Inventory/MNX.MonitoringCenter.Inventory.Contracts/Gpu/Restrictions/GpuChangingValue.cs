using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Изменяемые значения видеокарты.
/// </summary>
[ComplexType]
public record GpuChangingValue
{
    /// <summary>
    /// Закрытие ядра.
    /// </summary>
    public required RangeValue Lock { get; init; }

    /// <summary>
    /// Сдвиг ядра.
    /// </summary>
    public required RangeValue Offset { get; init; }
}
