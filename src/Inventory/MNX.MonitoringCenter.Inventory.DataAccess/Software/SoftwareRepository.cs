using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Software;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Software;

/// <summary>
/// Реализация <see cref="ISoftwareRepository"/>.
/// </summary>
public class SoftwareRepository : ISoftwareRepository
{
    private readonly Context _context;

    public SoftwareRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public Task<SoftwareInventory?> GetByRigId(InventorySpecification specification, CancellationToken cancellationToken)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification)
                                 .Include(x => x.Software)
                                 .Select(x => x.Software)
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
