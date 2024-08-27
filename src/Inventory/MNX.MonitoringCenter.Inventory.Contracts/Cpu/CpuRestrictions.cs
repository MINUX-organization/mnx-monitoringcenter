using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Ограничения процессора.
/// </summary>
[ComplexType]
public class CpuRestrictions
{
    /// <summary>
    /// Мощность.
    /// </summary>
    public RangeValue Power { get; set; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public RangeValue FanSpeed { get; set; }

    /// <summary>
    /// Температура.
    /// </summary>
    public RangeValue Temperature { get; set; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public RangeValue Clock { get; set; }
}
