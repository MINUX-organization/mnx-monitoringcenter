using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.Drive;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.Drive;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации жёстких дисков.
/// </summary>
public partial class InventoryRepository : IDriveRepository
{
    /// <inheritdoc/>
    public Task<List<Drive>?> GetDrives(InventorySpecification specification,
                                                            CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(specification)
                    .Include(x => x.Drives)
                    .Select(x => x.Drives)
                    .FirstOrDefaultAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task<int> GetDrivesCount(DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                    .Include(x => x.Drives)
                    .SelectMany(x => x.Drives)
                    .CountAsync(cancellationToken);
    }
}
