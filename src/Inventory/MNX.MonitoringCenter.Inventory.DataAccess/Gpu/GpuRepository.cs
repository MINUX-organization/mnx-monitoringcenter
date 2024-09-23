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
    public IAsyncEnumerable<List<Contracts.Gpu.Gpu>> GetSliceForAPeriod(InventorySpecification specification,
                                                                        DateTimeOffset startPeriod,
                                                                        DateTimeOffset endPeriod)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .InventoryFilter(specification)
                                 .GetForAPeriod(startPeriod, endPeriod)
                                 .Include(x => x.Gpus)
                                 .Select(x => x.Gpus)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<string> GetGpuUniqueNames(InventorySpecification specification)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .InventoryFilter(specification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Select(gpu => gpu.Information.Name)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetGpus(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Dictionary<string, int>> GetCountOfGpusGroupedByManufacturer(DeviceSpecification specification,
                                                                             CancellationToken cancellationToken)
    {
        return GetGpus(specification).GroupBy(x => x.Information.Manufacturer)
                                     .ToDictionaryAsync(x => x.Key, y => y.Count(), cancellationToken);
    }

    private IQueryable<Contracts.Gpu.Gpu> GetGpus(DeviceSpecification specification)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification.InventorySpecification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification.InventorySpecification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Filter(specification);
    }
}
