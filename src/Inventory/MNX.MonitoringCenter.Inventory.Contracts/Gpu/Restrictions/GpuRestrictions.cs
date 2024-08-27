using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;

/// <summary>
/// Ограничения видеокарты.
/// </summary>
[ComplexType]
public class GpuRestrictions
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
    public GpuTemperature Temperature { get; set; }

    /// <summary>
    /// Напряжение.
    /// </summary>
    public GpuVoltage Voltage { get; set; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public GpuClock Clock { get; set; }
}
