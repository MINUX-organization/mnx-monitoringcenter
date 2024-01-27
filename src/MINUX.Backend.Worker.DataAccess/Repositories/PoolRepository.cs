using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class PoolRepository : IPoolRepository
{
    private readonly Context _context;

    public PoolRepository(Context context)
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
