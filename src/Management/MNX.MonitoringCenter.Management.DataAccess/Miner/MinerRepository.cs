using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.DataAccess.Miner;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Реализация <see cref="IMinerRepository"/>.
/// </summary>
public class MinerRepository : IMinerRepository
{
    private readonly Context _context;

    public MinerRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Miner> GetAvailableMiners(Specification specification)
    {
        return _context.Miners
                       .AsNoTrackingWithIdentityResolution()
                       .Include(miner => miner.SupportedDevices)
                       .Filter(specification)
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Miner?> GetMinerById(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTrackingWithIdentityResolution()
                       .Include(miner => miner.SupportedDevices)
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.Id == id, cancellationToken);
    }
}