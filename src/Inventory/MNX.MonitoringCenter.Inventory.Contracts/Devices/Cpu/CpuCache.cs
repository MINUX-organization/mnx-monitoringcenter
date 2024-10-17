using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;

/// <summary>
/// Кеш процессора.
/// </summary>
[ComplexType]
public record CpuCache
{
    public int L1 { get; init; }

    public int L2 { get; init; }

    public int L3 { get; init; }

    public int? L4 { get; init; }
}
