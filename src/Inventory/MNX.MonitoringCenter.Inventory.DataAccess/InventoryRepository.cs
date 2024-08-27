using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases.SaveInventory;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

internal class InventoryRepository : IInventoryRepository
{
    private readonly Context _context;

    public InventoryRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task Save(Guid rigId, DateTimeOffset createdDate,
                           InventoryModel inventory, CancellationToken cancellationToken)
    {
        var oldInventory = await _context.Inventory.Where(x => x.RigId == rigId)
                                                   .FirstOrDefaultAsync(cancellationToken);

        if (oldInventory != null)
        {
            oldInventory.EndDate = createdDate;
        }

        await _context.Inventory.AddAsync(new Inventory()
        {
            RigId = rigId,
            CreatedDate = createdDate,
            Cpus = inventory.Cpus,
            Drives = inventory.Drives,
            Gpus = inventory.Gpus,
            InternetAdapters = inventory.InternetAdapters,
            Motherboard = inventory.Motherboard,
            Software = inventory.Software
        },
        cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
