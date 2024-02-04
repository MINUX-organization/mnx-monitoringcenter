using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

public class PoolRepository : IPoolRepository
{
    private readonly DataBaseContext _context;

    public PoolRepository(DataBaseContext context)
    {
        _context = context;
    }

    public IAsyncEnumerable<Pool> GetAll()
    {
        return _context.Pools.AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<Pool?> GetById(Guid id)
    {
        return await _context.Pools
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string domain, int port)
    {
        return await _context.Pools
                             .AsNoTracking()
                             .AnyAsync(x => x.Domain == domain && x.Port == port)
                             .ConfigureAwait(false);
    }

    public async Task<Guid> Add(Pool pool)
    {
        await _context.Pools.AddAsync(pool).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return pool.Id;
    }

    public async Task Update(Pool pool)
    {
        _context.Update(pool);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Remove(Pool pool)
    {
        _context.Pools.Remove(pool);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
