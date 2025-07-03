using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Gpu.Restrictions;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpuInfo;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Entities;
using MNX.MonitoringCenter.Inventory.DataAccess.Rigs.Devices.Gpu.Extensions;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Gpu;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации видеокарт.
/// </summary>
public partial class InventoryRepository : IGpuRepository
{
    /// <inheritdoc/>
    public async IAsyncEnumerable<GpuDetails> GetGpus(DeviceSpecification specification,
                                                      [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var query = from inventory in GetInventoryBySpecification(specification.InventorySpecification)
                    join gpuView in _context.GpuViews.AsNoTracking()
                         on inventory.Id equals gpuView.RigInventoryId
                    select gpuView;

        await foreach (var gpu in query.AsAsyncEnumerable().WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<GpuDetails>(gpu);
        }
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<List<Gpu>> GetGpusSliceForAPeriod(DeviceSpecification specification,
                                                                    DateTimeOffset startPeriod,
                                                                    DateTimeOffset endPeriod)
    {
        var inventories = GetInventorySliceForAPeriod(specification.InventorySpecification, startPeriod, endPeriod)
                          .Include(x => x.Gpus)
                          .AsAsyncEnumerable();

        await foreach (var inventory in inventories)
        {
            yield return _mapper.Map<List<Gpu>>(inventory.Gpus);
        }
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
            .SelectMany(inventory => inventory.Gpus)
            .ProjectTo<GpuInfo>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(gpu => gpu.Id == gpuId);
            
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictions(string gpuName)
    {
        if (string.IsNullOrWhiteSpace(gpuName))
        {
            return Task.FromResult<GpuRestrictions?>(null);
        }

        var manufacturer = gpuName.ToLower().Split().FirstOrDefault() ?? string.Empty;
        var model = string.Join(" ", gpuName.ToLower().Split().Skip(1));

        return _context.GpuRestrictionsView
            .AsNoTracking()
            .Where(x => x.Manufacturer.ToLower().Equals(manufacturer) &&
                         x.Model.ToLower().Equals(model))
            .OrderBy(x => x.RigInventoryId)
            .Select(x => _mapper.Map<GpuRestrictions>(x))
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public Task<GpuRestrictions?> GetGpusRestrictionsById(Guid gpuId)
    {
        return _context.GpuRestrictionsView.AsNoTracking()
            .Where(x => x.Id == gpuId)
            .OrderBy(x => x.RigInventoryId)
            .Select(x => _mapper.Map<GpuRestrictions>(x))
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
        return GetGpusList(specification).GroupBy(x => x.Information.Manufacturer.ToLower())
                                                .ToDictionaryAsync(x => x.Key, y => y.Count(), cancellationToken);
    }

    /// <summary>
    /// Получить список видеокарт.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Список видеокарт. </returns>
    private IQueryable<GpuInventory> GetGpusList(DeviceSpecification specification)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .SelectMany(x => x.Gpus.OfType<GpuInventory>())
                                 .Filter(specification);
    }
}
