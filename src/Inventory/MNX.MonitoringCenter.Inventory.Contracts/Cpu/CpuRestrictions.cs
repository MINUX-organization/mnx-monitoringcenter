using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Ограничения процессора.
/// </summary>
[ComplexType]
public record CpuRestrictions
{
    /// <summary>
    /// Мощность.
    /// </summary>
    public required RangeValue Power { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public required RangeValue FanSpeed { get; init; }

    /// <summary>
    /// Температура.
    /// </summary>
    public required RangeValue Temperature { get; init; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public required RangeValue Clock { get; init; }
}
