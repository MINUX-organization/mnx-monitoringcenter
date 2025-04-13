using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpuInfo;
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
                                     Overclocking = gpu.Overclocking,
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
    public Task<GpuInfo?> GetInfo(Guid gpuId, Guid userId)
    {
        return GetInventoryBySpecification(new InventorySpecification(userId))
            .Include(inventory => inventory.Gpus)
            .SelectMany(inventory => inventory.Gpus.Select(gpu => new GpuInfo()
            {
                Id = gpu.Id,
                Manufacturer = gpu.Information.Manufacturer,
                Model = gpu.Information.Model,
                SerialNumber = gpu.Information.SerialNumber,
                Vendor = gpu.Information.Vendor,
                BiosVersion = gpu.Information.BiosVersion,
                Technology = gpu.Information.Technology,
                Memory = gpu.Information.Memory
            }))
            .FirstOrDefaultAsync(gpu => gpu.Id == gpuId);
            
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictions(string gpuName)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
        {
            return Task.FromResult<GpuRestrictions?>(null);
        }

        var manufacturer = gpuName.ToLower().Split().First();
        var model = string.Join(" ", gpuName.ToLower().Split().Skip(1));

        return _context.Gpu
            .AsNoTracking()
            .Where(x => x.Information.Manufacturer.ToLower().Equals(manufacturer) &&
                         x.Information.Model.ToLower().Equals(model))
            .Select(x => x.Restrictions)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictionsById(Guid gpuId)
    {
        return _context.Gpu
            .AsNoTracking()
            .OrderBy(x => x.Id)
            .Where(x => x.Id == gpuId)
            .Select(x => x.Restrictions)
            .LastOrDefaultAsync();
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
