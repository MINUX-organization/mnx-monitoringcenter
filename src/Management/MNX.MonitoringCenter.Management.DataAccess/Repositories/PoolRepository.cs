using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Pool;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

/// <summary>
/// Реализует <see cref="IPoolRepository"/>
/// </summary>
public class PoolRepository : IPoolRepository
{
    private readonly Context _context;

    public PoolRepository(Context context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Pool> GetAllAvailable(long userId)
    {
        return _context.Pools.Include(x => x.Cryptocurrency)
                             .Where(x => x.UserId == userId)
                             .AsNoTracking()
                             .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Pool?> GetAvailableById(Guid id, long userId)
    {
        return await _context.Pools
                             .Include(x => x.Cryptocurrency)
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(long userId, string domain, int port)
    {
        return await _context.Pools
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .AnyAsync(x => x.Domain == domain && x.Port == port)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Guid> Add(Pool pool)
    {
        await _context.Pools.AddAsync(pool).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return pool.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Pool pool)
    {
        _context.Update(pool);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task Remove(Pool pool)
    {
        _context.Pools.Remove(pool);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
