using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Реализация <see cref="IRigRepository"/>.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly InventoryRepository _inventoryRepository;

    public RigRepository(Context context, IMapper mapper)
    {
        _inventoryRepository = new InventoryRepository(context, mapper);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<RigDetails> GetRigs(InventorySpecification specification)
    {
        var rigsQuery = _inventoryRepository.GetRigsBySpecification(specification)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.Software)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.Software.Miners)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.Cpus)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.Drives)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.Gpus)

                            .Include(rig => rig.Inventories.Where(x => x.IsCurrent))
                                .ThenInclude(inventory => inventory.NetworkAdapters.Where(x => x.GlobalIP != null))

                            .Where(rig => rig.Inventories.Count != 0);

        await foreach (var rig in rigsQuery.AsAsyncEnumerable())
        {
            yield return new RigDetails()
            {
                Id = rig.Id,
                OwnerId = rig.OwnerId,
                Name = rig.Name,
                Mac = rig.CurrentInventory!.NetworkAdapters.FirstOrDefault()?.Information.Mac,
                GlobalIP = rig.CurrentInventory!.NetworkAdapters.FirstOrDefault()?.GlobalIP!,
                LocalIP = rig.CurrentInventory!.NetworkAdapters.FirstOrDefault()?.LocalIP!,
                Software = rig.CurrentInventory!.Software,
                CountDevices = new ModelWithCountDevices()
                {
                    TotalCpusCountGroupedByManufacturer = rig.CurrentInventory.Cpus
                                                                .GroupBy(x => x.Information.Manufacturer)
                                                                .ToDictionary(x => x.Key.ToLower(), y => y.Count()),

                    TotalGpusCountGroupedByManufacturer = rig.CurrentInventory.Gpus
                                                                .GroupBy(x => x.Information.Manufacturer)
                                                                .ToDictionary(x => x.Key.ToLower(), y => y.Count()),

                    TotalDrivesCount = rig.CurrentInventory.Drives.Count,
                }
            };
        }

        // todo: поддержка GroupBy для сложных типов присутствует в EF 9. Это должно помочь не загружать лишних данных.
        //       https://learn.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-9.0/whatsnew#complex-types-groupby-and-executeupdate-support
    }

    /// <inheritdoc/>
    public Task<Guid[]> GetRigIdsByMinerCoincidence(string minerName,
                                                    string minerVersion,
                                                    Guid userId,
                                                    CancellationToken cancellationToken)
    {
        var targetMiner = new KeyValuePair<string, string>(minerName, minerVersion);
        return _inventoryRepository.GetMatchingRigIdsQuery(targetMiner, userId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<Guid[]> GetRigIdsWithoutMiner(Guid[] rigIds,
                                              Guid userId,
                                              string minerName,
                                              string minerVersion,
                                              CancellationToken cancellationToken)
    {
        var targetMiner = new KeyValuePair<string, string>(minerName, minerVersion);
        return _inventoryRepository.GetRigIdsWithoutMiner(rigIds, userId, targetMiner, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid rigId, CancellationToken cancellationToken)
    {
        return _inventoryRepository.Exists(rigId, cancellationToken);
    }

    /// <inheritdoc/>
    public Task Add(Rig rig, CancellationToken cancellationToken)
    {
        return _inventoryRepository.AddRig(rig, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<long> SaveInventory(Guid rigId, DateTimeOffset createdDate,
                                    RigInventoryModel inventory, CancellationToken cancellationToken)
    {
        return _inventoryRepository.Save(rigId, createdDate, inventory, cancellationToken);
    }

    /// <inheritdoc/>
    public Task SetInventoryExpirationDate(Guid rigId, CancellationToken cancellationToken)
    {
        return _inventoryRepository.SetExpirationDate(rigId, cancellationToken);
    }
}
