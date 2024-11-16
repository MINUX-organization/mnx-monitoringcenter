using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Pool;

namespace MNX.MonitoringCenter.Management.DataAccess.Pool;

using Pool = Core.Pool;

/// <summary>
/// Реализует <see cref="IPoolRepository"/>
/// </summary>
public class PoolRepository : IPoolRepository
{
    private readonly Context _context;

    public PoolRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Pool> GetAllAvailable(Specification specification)
    {
        return _context.Pools.Include(x => x.Cryptocurrency)
                             .Where(x => x.UserId == specification.UserId)
                             .Filter(specification)
                             .AsNoTrackingWithIdentityResolution()
                             .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Pool?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.Pools.Include(x => x.Cryptocurrency)
                                .ThenInclude(c => c!.Algorithm)
                             .AsNoTrackingWithIdentityResolution()
                             .Where(x => x.UserId == userId)
                             .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId, string domain, int port, CancellationToken cancellationToken)
    {
        return _context.Pools.AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .AnyAsync(x => x.Domain == domain && x.Port == port, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(Pool pool)
    {
        await _context.Pools.AddAsync(pool);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Update(Pool pool)
    {
        _context.Pools.Update(pool);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId)
    {
        return _context.Pools
            .Where(x => x.Id == id && x.UserId == userId)
            .ExecuteDeleteAsync();
    }
}
