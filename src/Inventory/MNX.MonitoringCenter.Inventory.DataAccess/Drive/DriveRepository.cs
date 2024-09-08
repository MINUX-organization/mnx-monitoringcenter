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
    public Task<List<Contracts.Drive.Drive>?> GetList(InventorySpecification specification,
                                                      CancellationToken cancellationToken)
    {
        return GetInventory(specification).Include(x => x.Drives)
                                          .Select(x => x.Drives)
                                          .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<int> GetCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetInventory(specification.InventorySpecification).Include(x => x.Drives)
                                                                 .SelectMany(x => x.Drives)
                                                                 .CountAsync(cancellationToken);
    }

    private IQueryable<Inventory> GetInventory(InventorySpecification specification)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification);
    }
}
