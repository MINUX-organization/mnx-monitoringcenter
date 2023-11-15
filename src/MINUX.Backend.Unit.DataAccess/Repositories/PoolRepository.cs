using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

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
        await _context.Pools.AddAsync(pool);
        return pool.Id;
    }

    public void Remove(Pool pool)
    {
        _context.Pools.Remove(pool);
    }
}
