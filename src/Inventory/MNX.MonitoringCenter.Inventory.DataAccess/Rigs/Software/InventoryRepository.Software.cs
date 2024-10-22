using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.UseCases.Software;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации программного обеспечения.
/// </summary>
public partial class InventoryRepository : ISoftwareRepository
{
    /// <inheritdoc/>
    public Task<SoftwareInventory?> GetSoftwareByRigId(Guid rigId, Guid userId, CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(new InventorySpecification(userId, rigId))
                                 .Include(x => x.Software)
                                 .Select(x => _mapper.Map<SoftwareInventory>(x.Software))
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
