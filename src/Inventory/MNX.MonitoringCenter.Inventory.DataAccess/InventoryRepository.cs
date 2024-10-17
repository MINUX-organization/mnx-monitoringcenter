using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.RigInventory;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации.
/// </summary>
public partial class InventoryRepository
{
    private readonly Context _context;

    public InventoryRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    internal async Task Save(Guid rigId, DateTimeOffset createdDate,
                             RigInventoryModel inventory, CancellationToken cancellationToken)
    {
        var oldInventory = await GetInventoryBySpecification(new InventorySpecification(null, rigId, true))
                                    .FirstOrDefaultAsync(cancellationToken);

        var newInventory = MapInventory(rigId, createdDate, inventory);

        if (newInventory.Equals(oldInventory))
        {
            return;
        }

        if (oldInventory != null)
        {
            oldInventory.EndDateTime = createdDate;
            _context.RigInventory.Update(oldInventory);
        }

        await _context.RigInventory.AddAsync(newInventory, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// Получить срез инвентаризации за период.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <param name="startPeriod"> Начало периода. </param>
    /// <param name="endPeriod"> Конец периода. </param>
    /// <returns> Инвентаризация. </returns>
    private IQueryable<RigInventory.RigInventory> GetInventorySliceForAPeriod(InventorySpecification specification,
                                                                              DateTimeOffset startPeriod,
                                                                              DateTimeOffset endPeriod)
    {
        return GetInventoryBySpecification(specification).GetForAPeriod(startPeriod, endPeriod);
    }

    /// <summary>
    /// Получение инвентаризации по спецификации.
    /// </summary>
    /// <param name="specification"> Спецификация. </param>
    /// <returns> Инвентаризация. </returns>
    private IQueryable<RigInventory.RigInventory> GetInventoryBySpecification(InventorySpecification specification)
    {
        return _context.RigInventory.AsNoTrackingWithIdentityResolution()
                                    .Include(inventory => inventory.Rig)
                                    .Where(inventory => GetRigsBySpecification(specification).Contains(inventory.Rig))
                                    .Actualize(specification);
    }

    private static RigInventory.RigInventory MapInventory(Guid rigId, DateTimeOffset createdDate, 
                                                          RigInventoryModel inventory)
    {
        return new RigInventory.RigInventory()
        {
            RigId = rigId,
            CreatedDateTime = createdDate,
            Cpus = inventory.Cpus,
            Drives = inventory.Drives,
            Gpus = inventory.Gpus,
            NetworkAdapters = inventory.NetworkAdapters,
            Motherboard = inventory.Motherboard,
            Software = inventory.Software
        };
    }
}
