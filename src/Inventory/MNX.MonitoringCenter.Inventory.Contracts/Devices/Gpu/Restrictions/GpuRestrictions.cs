using System.ComponentModel.DataAnnotations.Schema;
using MNX.MonitoringCenter.Inventory.Contracts.Devices;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения видеокарты.
/// </summary>
[ComplexType]
public record GpuRestrictions
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
    public required GpuTemperature Temperature { get; init; }

    /// <summary>
    /// Напряжение.
    /// </summary>
    public required GpuVoltage Voltage { get; init; }

    /// <summary>
    /// Разгон.
    /// </summary>
    public required GpuClock Clock { get; init; }
}
