using MessagePack;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Overclocking;

using Overclocking = Devices.Overclocking;

/// <summary>
/// Разгон видеокарт.
/// </summary>
[Union(0, typeof(NvidiaGpuOverclocking))]
[Union(1, typeof(AmdGpuOverclocking))]
[Union(2, typeof(IntelGpuOverclocking))]
public abstract record GpuOverclocking : Overclocking
{
    /// <summary>
    /// Мощность.
    /// </summary>
    public int? PowerLimit { get; init; }

    /// <summary>
    /// Скорость вентилятора.
    /// </summary>
    public int? FanSpeed { get; init; }
}
