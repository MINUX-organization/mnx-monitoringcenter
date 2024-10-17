using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Rigs;

/// <summary>
/// Реализация <see cref="IRigRepository"/>.
/// </summary>
public class RigRepository : IRigRepository
{
    private readonly InventoryRepository _inventoryRepository;

    public RigRepository(Context context)
    {
        _inventoryRepository = new InventoryRepository(context);
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Rig> GetRigs(InventorySpecification specification)
    {
        return _inventoryRepository.GetRigsBySpecification(specification)
                    .Select(dto => new Rig()
                    {
                        Id = dto.Id,
                        OwnerId = dto.OwnerId,
                        Name = dto.Name
                    })
                    .AsAsyncEnumerable();
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
