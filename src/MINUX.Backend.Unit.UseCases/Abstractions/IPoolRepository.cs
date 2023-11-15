using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IPoolRepository : IRepository
{
    IAsyncEnumerable<Pool> GetAll();

    Task<Guid> Add(Pool pool);

    void Remove(Pool pool);
}
