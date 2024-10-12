using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Queries;
using MNX.MonitoringCenter.Inventory.UseCases;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Реализация <see cref="IInventoryRepository"/>.
/// </summary>
public class InventoryRepository : IInventoryRepository
{
    private readonly Context _context;

    public InventoryRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public async Task Save(Guid ownerId, Guid rigId, DateTimeOffset createdDate,
                           InventoryModel inventory, CancellationToken cancellationToken)
    {
        var oldInventory = await _context.Inventory
            .GetCurrentInventory()
            .InventoryFilter(new InventorySpecification(ownerId, rigId))
            .FirstOrDefaultAsync(cancellationToken);

        var newInventory = new Inventory()
        {
            RigOwnerId = ownerId,
            RigId = rigId,
            CreatedDateTime = createdDate,
            Cpus = inventory.Cpus,
            Drives = inventory.Drives,
            Gpus = inventory.Gpus,
            NetworkAdapters = inventory.NetworkAdapters,
            Motherboard = inventory.Motherboard,
            Software = inventory.Software
        };

        if (newInventory.Equals(oldInventory))
        {
            return;
        }

        if (oldInventory != null)
        {
            oldInventory.EndDateTime = createdDate;
        }

        await _context.Inventory.AddAsync(newInventory, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
