namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;

/// <summary>
/// Абстрактный класс с общими параметрами ограничений разгона видеокарт.
/// </summary>
public abstract class GpuRestrictionsInventory
{
    /// <summary>
    /// Ограничения мощности.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory Power { get; init; }

    /// <summary>
    /// Ограничения скорости вентилятора
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory FanSpeed { get; init; }

    /// <summary>
    /// Ограничения температуры процессора.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory TemperatureCore { get; init; }

    /// <summary>
    /// Ограничения температуры памяти.
    /// </summary>
    public required GpuIntegerTypeRestrictionsInventory TemperatureMemory { get; init; }
}
