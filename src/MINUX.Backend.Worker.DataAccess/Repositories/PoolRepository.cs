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

    public async Task<Guid> Add(Pool pool)
    {
        await _context.Pools.AddAsync(pool).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return pool.Id;
    }

    public async Task Remove(Pool pool)
    {
        _context.Pools.Remove(pool);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
