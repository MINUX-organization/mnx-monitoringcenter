using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Inventory.UseCases;
using MNX.MonitoringCenter.Inventory.UseCases.NetworkAdapter;

namespace MNX.MonitoringCenter.Inventory.DataAccess.NetworkAdapter;

/// <summary>
/// Реализация <see cref="INetworkAdapterRepository"/>.
/// </summary>
public class NetworkAdapterRepository : INetworkAdapterRepository
{
    private readonly Context _context;

    public NetworkAdapterRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public Task<List<Contracts.NetworkAdapter.NetworkAdapter>?> GetList(Guid rigId,
                                                                        CancellationToken cancellationToken)
    {
        return _context.Inventory.AsNoTrackingWithIdentityResolution()
                                 .GetCurrentInventory()
                                 .InventoryFilter(new InventorySpecification(rigId))
                                 .Include(x => x.NetworkAdapters)
                                 .Select(x => x.NetworkAdapters)
                                 .FirstOrDefaultAsync(cancellationToken);
    }
}
