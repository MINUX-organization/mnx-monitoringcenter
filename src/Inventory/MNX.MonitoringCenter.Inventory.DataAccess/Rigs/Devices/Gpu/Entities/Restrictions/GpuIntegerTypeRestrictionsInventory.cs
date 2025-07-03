using Microsoft.EntityFrameworkCore;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities.Restrictions;

/// <summary>
/// Ограничения параметров разгона типа <see cref="int"/>.
/// </summary>
[Owned]
public class GpuIntegerTypeRestrictionsInventory
{
    public int? Minimal { get; init; }

    public int? Maximal { get; init; }

    public int? Default { get; init; }

    public bool? IsWritable { get; init; }
}
