using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации видеокарт.
/// </summary>
public partial class InventoryRepository : IGpuRepository
{
    /// <inheritdoc/>
    public IAsyncEnumerable<Contracts.Gpu.Gpu> GetGpus(DeviceSpecification specification)
    {
        return GetGpusList(specification).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<List<Contracts.Gpu.Gpu>> GetGpusSliceForAPeriod(InventorySpecification specification,
                                                                            DateTimeOffset startPeriod,
                                                                            DateTimeOffset endPeriod)
    {
        return GetInventorySliceForAPeriod(specification, startPeriod, endPeriod)
                                 .Include(x => x.Gpus)
                                 .Select(x => x.Gpus)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<string> GetGpusUniqueNames(InventorySpecification specification)
    {
        return GetInventoryBySpecification(specification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Select(gpu => gpu.Information.Name)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictions(string gpuName)
    {
        var manufacturer = gpuName.ToLower().Split().First();
        var model = string.Join(" ", gpuName.ToLower().Split().Skip(1));

        return _context.Gpu.Where(x => x.Information.Manufacturer.ToLower() == manufacturer &&
                                       x.Information.Model.ToLower() == model)
                           .Select(x => x.Restrictions)
                           .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<int> GetGpusCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetGpusList(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Dictionary<string, int>> GetGpusCountGroupedByManufacturer(DeviceSpecification specification,
                                                                           CancellationToken cancellationToken)
    {
        return GetGpusList(specification).GroupBy(x => x.Information.Manufacturer)
                                     .ToDictionaryAsync(x => x.Key, y => y.Count(), cancellationToken);
    }

    /// <inheritdoc/>
    private IQueryable<Contracts.Gpu.Gpu> GetGpusList(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Filter(specification);
    }
}
