using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Queries;
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
    public Task<Contracts.Motherboard.Motherboard?> GetByRigId(Guid rigId, Guid userId,
                                                               CancellationToken cancellationToken)
    {
        var specification = new InventorySpecification(userId, rigId);

        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .Available(specification)
                                 .GetCurrentInventory()
                                 .InventoryFilter(specification)
                                 .Include(inventory => inventory.Motherboard)
                                    .ThenInclude(motherboard => motherboard.Pcies)
                                 .Select(inventory => inventory.Motherboard)
                                 .FirstOrDefaultAsync(cancellationToken);

    }
}
