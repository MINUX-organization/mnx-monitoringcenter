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
                       .Include(miner => miner.SupportedAlgorithms)
                       .Filter(specification)
                       .Sort()
                       .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Miner?> GetMinerById(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTrackingWithIdentityResolution()
                       .Include(miner => miner.SupportedAlgorithms)
                       .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId,
                             string minerName,
                             string minerVersion,
                             CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.OwnerId == userId &&
                                      x.Name == minerName &&
                                      x.Version == minerVersion,
                       cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid id, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid minerId, Guid userId, string minerName, CancellationToken cancellationToken)
    {
        return _context.Miners
                       .AsNoTracking()
                       .AnyAsync(x => x.OwnerId == userId &&
                                 x.Name == minerName &&
                                 x.Id != minerId,
                                 cancellationToken);
    }

    /// <inheritdoc/>
    public Task Add(Miner miner, CancellationToken cancellationToken)
    {
        _context.Miners.Add(miner);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Edit(Miner miner, CancellationToken cancellationToken)
    {
        _context.Miners.Update(miner);
        return _context.SaveChangesAsync(cancellationToken);
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.Miners
            .Where(miner => miner.Id == id && miner.OwnerId == userId)
            .ExecuteDeleteAsync(cancellationToken);
    }
}