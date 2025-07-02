using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Overclocking;

/// <summary>
/// Абстрактный класс с общими параметрами разгона видеокарт.
/// </summary>
public abstract class GpuOverclockingInventory
{
    /// <summary>
    /// Мощность.
    /// </summary>
    [Column("power_limit")]
    public int? PowerLimit { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    [Column("fan_speed")]
    public int? FanSpeed { get; init; }
}
