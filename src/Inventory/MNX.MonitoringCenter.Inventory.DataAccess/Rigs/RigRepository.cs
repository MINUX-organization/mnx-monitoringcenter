using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases;
using AutoMapper;

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
    public IAsyncEnumerable<RigDetails> GetRigs(InventorySpecification specification)
    {
        return _inventoryRepository.GetRigsBySpecification(specification)
                    .Include(rig => rig.Inventories)
                        .ThenInclude(inventory => inventory.Software)
                    .Where(rig => rig.Inventories.Count != 0)
                    .Select(rig => new RigDetails()
                    {
                        Id = rig.Id,
                        OwnerId = rig.OwnerId,
                        Name = rig.Name,
                        Software = _mapper.Map<SoftwareInventory>(rig.CurrentInventory!.Software)
                    })
                    .AsAsyncEnumerable();
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
