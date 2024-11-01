using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Inventory.DataAccess.RigInventory.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации видеокарт.
/// </summary>
public partial class InventoryRepository : IGpuRepository
{
    /// <inheritdoc/>
    public IAsyncEnumerable<GpuDetails> GetGpus(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(inventory => inventory.Gpus)
                                 .Include(inventory => inventory.Software)
                                 .SelectMany(inventory => inventory.Gpus.Select(gpu => new GpuDetails()
                                 {
                                     Id = gpu.Id,
                                     RigName = inventory.Rig!.Name,
                                     Information = gpu.Information,
                                     Pci = gpu.Pci,
                                     Restrictions = gpu.Restrictions,
                                     DriverVersion = Context
                                        .GetGpuDriverVersion(inventory.Software.AmdGpuDriverVersion,
                                                             inventory.Software.IntelGpuDriverVersion,
                                                             inventory.Software.NvidiaGpuDriverVersion,
                                                             gpu.Information.Manufacturer)
                                 })) 
                                 .Filter(specification)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<List<Gpu>> GetGpusSliceForAPeriod(DeviceSpecification specification,
                                                              DateTimeOffset startPeriod,
                                                              DateTimeOffset endPeriod)
    {
        return GetInventorySliceForAPeriod(specification.InventorySpecification, startPeriod, endPeriod)
                                 .Include(x => x.Gpus)
                                 .Select(x => x.Gpus)
                                 .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<string> GetGpusUniqueNames(DeviceSpecification specification)
    {
        var nameSet = new HashSet<string>();

        var stream = GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Select(gpu => gpu.Information.Name)
                                 .AsAsyncEnumerable();

        await foreach (var name in stream)
        {
            if (nameSet.Contains(name))
                continue;

            nameSet.Add(name);
            yield return name;
        }

        // todo: найти способ правильной трансляции Distinct() в sql
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictions(string gpuName)
    {
        var manufacturer = gpuName.ToLower().Split().First();
        var model = string.Join(" ", gpuName.ToLower().Split().Skip(1));

        return _context.Gpu
            .Where(x => x.Information.Manufacturer.ToLower().Equals(manufacturer) &&
                         x.Information.Model.ToLower().Equals(model))
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
                                     .ToDictionaryAsync(x => x.Key.ToLower(), y => y.Count(), cancellationToken);
    }

    /// <summary>
    /// Получить список видеокарт.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список видеокарт. </returns>
    private IQueryable<Gpu> GetGpusList(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(x => x.Gpus)
                                 .SelectMany(x => x.Gpus)
                                 .Filter(specification);
    }
}
