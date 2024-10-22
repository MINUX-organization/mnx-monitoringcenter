using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusInfo;
using MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Cpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации процессоров.
/// </summary>
public partial class InventoryRepository : ICpuRepository
{
    /// <inheritdoc/>
    public IAsyncEnumerable<CpuModel> GetCpus(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(inventory => inventory.Cpus)
                                 .SelectMany(inventory => inventory.Cpus.Select(cpu => new CpuModel()
                                 {
                                     Id = cpu.Id,
                                     RigName = inventory.Rig!.Name,
                                     Information = cpu.Information,
                                     Pci = cpu.Pci,
                                     Restrictions = cpu.Restrictions
                                 }))
                                 .Filter(specification)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<List<Cpu>> GetCpusSliceForAPeriod(InventorySpecification specification,
                                                                            DateTimeOffset startPeriod,
                                                                            DateTimeOffset endPeriod)
    {
        return GetInventorySliceForAPeriod(specification, startPeriod, endPeriod)
                                 .Include(x => x.Cpus)
                                 .Select(x => x.Cpus)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<int> GetCpusCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetCpusList(specification).CountAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Dictionary<string, int>> GetCpusCountGroupedByManufacturer(DeviceSpecification specification,
                                                                           CancellationToken cancellationToken)
    {
        return GetCpusList(specification).GroupBy(x => x.Information.Manufacturer)
                                     .ToDictionaryAsync(x => x.Key.ToLower(), y => y.Count(), cancellationToken);
    }

    /// <inheritdoc/>
    private IQueryable<Cpu> GetCpusList(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(x => x.Cpus)
                                 .SelectMany(x => x.Cpus)
                                 .Filter(specification);
    }
}
