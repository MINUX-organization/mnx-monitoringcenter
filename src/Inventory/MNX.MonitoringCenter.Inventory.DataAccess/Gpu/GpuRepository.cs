using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Gpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Gpu;

/// <summary>
/// Реализация <see cref="IGpuRepository"/>.
/// </summary>
public class GpuRepository : IGpuRepository
{
    private readonly Context _context;

    public GpuRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Contracts.Gpu.Gpu> GetList(DeviceSpecification specification)
    {
        return GetGpus(specification).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<int> GetCount(DeviceSpecification specification)
    {
        return GetGpus(specification).CountAsync();
    }

    private IQueryable<Contracts.Gpu.Gpu> GetGpus(DeviceSpecification specification)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification.InventorySpecification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Filter(specification);
    }
}
