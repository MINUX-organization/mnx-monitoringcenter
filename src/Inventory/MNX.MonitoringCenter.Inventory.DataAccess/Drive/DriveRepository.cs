using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Drive;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Drive;

/// <summary>
/// Реализация <see cref="IDriveRepository"/>.
/// </summary>
public class DriveRepository : IDriveRepository
{
    private readonly Context _context;

    public DriveRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public Task<List<Contracts.Drive.Drive>?> GetList(Guid rigId, CancellationToken cancellationToken)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .GetCurrentInventory()
                                 .InventoryFilter(new InventorySpecification(rigId))
                                 .Include(x => x.Drives)
                                 .Select(x => x.Drives)
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
