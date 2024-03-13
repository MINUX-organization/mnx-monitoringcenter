using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

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
    public IAsyncEnumerable<Pool> GetAll()
    {
        return _context.Pools.Include(x => x.Cryptocurrency)
                             .AsNoTracking()
                             .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Pool?> GetById(Guid id)
    {
        return await _context.Pools
                             .Include(x => x.Cryptocurrency)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(string domain, int port)
    {
        return await _context.Pools
                             .AsNoTracking()
                             .AnyAsync(x => x.Domain == domain && x.Port == port)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task Add(Pool pool)
    {
        await _context.Pools.AddAsync(pool).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
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
