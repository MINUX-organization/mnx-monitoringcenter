using MessagePack;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;

/// <summary>
/// Ограничения параметров видеокарт.
/// </summary>
[Union(0, typeof(NvidiaGpuRestrictions))]
[Union(1, typeof(AmdGpuRestrictions))]
[Union(2, typeof(IntelGpuRestrictions))]
public abstract record GpuRestrictions : Devices.Restrictions
{
    /// <summary>
    /// Ограничения мощности.
    /// </summary>
    public required IntegerTypeRestrictions Power { get; init; }

    /// <summary>
    /// Ограничения скорости вентилятора.
    /// </summary>
    public required IntegerTypeRestrictions FanSpeed { get; init; }

    /// <summary>
    /// Ограничения температуры процессора.
    /// </summary>
    public required IntegerTypeRestrictions TemperatureCore { get; init; }
    
    /// <summary>
    /// Ограничения температуры памяти.
    /// </summary>
    public required IntegerTypeRestrictions TemperatureMemory { get; init; }
};
