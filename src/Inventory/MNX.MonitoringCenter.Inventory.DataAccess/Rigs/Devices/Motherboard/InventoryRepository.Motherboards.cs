using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Motherboard;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.Motherboard;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации материнских плат.
/// </summary>
public partial class InventoryRepository : IMotherboardRepository
{
    /// <inheritdoc/>
    public Task<Motherboard?> GetMotherboardByRigId(Guid rigId, Guid ownerId,
                                                    CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(new InventorySpecification(ownerId, rigId, true))
                                 .Include(inventory => inventory.Motherboard)
                                    .ThenInclude(motherboard => motherboard.Pcies)
                                 .Select(inventory => inventory.Motherboard)
                                 .FirstOrDefaultAsync(cancellationToken);

    }
}
