using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.Contracts.Devices.NetworkAdapter;
using MNX.MonitoringCenter.Inventory.Contracts.Requests;
using MNX.MonitoringCenter.Inventory.UseCases.Devices.NetworkAdapter;

namespace MNX.MonitoringCenter.Inventory.DataAccess;

/// <summary>
/// Репозиторий инвентаризации сетевых адаптеров.
/// </summary>
public partial class InventoryRepository : INetworkAdapterRepository
{
    /// <inheritdoc/>
    public Task<List<NetworkAdapter>?> GetNetworkAdapters
        (DeviceSpecification specification, CancellationToken cancellationToken)
    {
        return GetInventoryBySpecification(specification.InventorySpecification)
                                 .Include(x => x.NetworkAdapters)
                                 .Select(x => x.NetworkAdapters)
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
