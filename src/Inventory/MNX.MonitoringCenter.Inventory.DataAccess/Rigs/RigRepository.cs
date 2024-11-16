using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.CountDevices;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Реализация <see cref="IRigRepository"/>.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly IMapper _mapper;

    private readonly InventoryRepository _inventoryRepository;

    public RigRepository(Context context, IMapper mapper)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _inventoryRepository = new InventoryRepository(context, mapper);
    }

    /// <inheritdoc/>
    public async IAsyncEnumerable<RigDetails> GetRigs(InventorySpecification specification)
    {
        var rigsQuery = _inventoryRepository.GetRigsBySpecification(specification)

                            .Include(rig => rig.Inventories.Where(x => x.EndDateTime == null))
                                .ThenInclude(inventory => inventory.Software)

                            .Include(rig => rig.Inventories.Where(x => x.EndDateTime == null))
                                .ThenInclude(inventory => inventory.Cpus)

                            .Include(rig => rig.Inventories.Where(x => x.EndDateTime == null))
                                .ThenInclude(inventory => inventory.Drives)

                            .Include(rig => rig.Inventories.Where(x => x.EndDateTime == null))
                                .ThenInclude(inventory => inventory.Gpus)

                            .Include(rig => rig.Inventories.Where(x => x.EndDateTime == null))
                                .ThenInclude(inventory => inventory.NetworkAdapters.Where(x => x.GlobalIP != null))

                            .Where(rig => rig.Inventories.Count != 0);

        await foreach (var rig in rigsQuery.AsAsyncEnumerable())
        {
            yield return new RigDetails()
            {
                Id = rig.Id,
                OwnerId = rig.OwnerId,
                Name = rig.Name,
                Mac = rig.CurrentInventory!.NetworkAdapters.First().Information.Mac,
                GlobalIP = rig.CurrentInventory!.NetworkAdapters.First().GlobalIP!,
                LocalIP = rig.CurrentInventory!.NetworkAdapters.First().LocalIP!,
                Software = _mapper.Map<SoftwareInventory>(rig.CurrentInventory!.Software),
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
    public Task SaveInventory(Guid rigId, DateTimeOffset createdDate,
                              RigInventoryModel inventory, CancellationToken cancellationToken)
    {
        return _inventoryRepository.Save(rigId, createdDate, inventory, cancellationToken);
    }
}
