using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.UseCases.RigInventory.Devices.NetworkAdapter;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации сетевых адаптеров.
/// </summary>
public partial class InventoryRepository : INetworkAdapterRepository
{
    /// <inheritdoc/>
    public Task<List<Contracts.NetworkAdapter.NetworkAdapter>?> GetNetworkAdapters
        (InventorySpecification specification, CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(specification)
                                 .Include(x => x.NetworkAdapters)
                                 .Select(x => x.NetworkAdapters)
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
