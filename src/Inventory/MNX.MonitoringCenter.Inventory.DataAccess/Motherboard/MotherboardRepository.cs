using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.Motherboard;

namespace MNX.MonitoringCenter.Inventory.DataAccess.Motherboard;

/// <summary>
/// Реализация <see cref="IMotherboardRepository"/>.
/// </summary>
public class MotherboardRepository : IMotherboardRepository
{
    private readonly Context _context;

    public MotherboardRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public Task<Contracts.Motherboard.Motherboard?> GetByRigId(InventorySpecification specification,
                                                               CancellationToken cancellationToken)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification)
                                 .Include(x => x.Motherboard)
                                 .Select(x => x.Motherboard)
                                 .FirstOrDefaultAsync(cancellationToken);

    }
}
