using System.ComponentModel.DataAnnotations.Schema;

namespace MNX.MonitoringCenter.Inventory.Contracts.Cpu;

/// <summary>
/// Кеш процессора.
/// </summary>
[ComplexType]
public sealed record CpuCache
{
    public int L1 { get; set; }

    public int L2 { get; set; }

    public int L3 { get; set; }
}
