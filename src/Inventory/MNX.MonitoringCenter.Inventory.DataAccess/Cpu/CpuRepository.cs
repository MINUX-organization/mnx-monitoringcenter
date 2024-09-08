using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Cpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Cpu;

/// <summary>
/// Реализация <see cref="ICpuRepository"/>.
/// </summary>
public class CpuRepository : ICpuRepository
{
    private readonly Context _context;

    public CpuRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Contracts.Cpu.Cpu> GetList(DeviceSpecification specification)
    {
        return GetCpus(specification).AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<List<Contracts.Cpu.Cpu>> GetSliceForAPeriod(InventorySpecification specification,
                                                                        DateTimeOffset startPeriod,
                                                                        DateTimeOffset endPeriod)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .InventoryFilter(specification)
                                 .GetForAPeriod(startPeriod, endPeriod)
                                 .Include(x => x.Cpus)
                                 .Select(x => x.Cpus)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetCpus(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Dictionary<string, int>> GetCountOFCpusGroupedByManufacturer(DeviceSpecification specification,
                                                                             CancellationToken cancellationToken)
    {
        return GetCpus(specification).GroupBy(x => x.Information.Manufacturer)
                                     .ToDictionaryAsync(x => x.Key, y => y.Count(), cancellationToken);
    }

    private IQueryable<Contracts.Cpu.Cpu> GetCpus(DeviceSpecification specification)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification.InventorySpecification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification.InventorySpecification)
                                 .Include(x => x.Cpus)
                                 .SelectMany(x => x.Cpus)
                                 .Filter(specification);
    }
}
