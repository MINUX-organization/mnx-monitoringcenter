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

    public Task Add(Pool cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Pool> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}
