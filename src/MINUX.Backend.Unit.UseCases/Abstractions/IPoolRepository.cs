using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IPoolRepository
{
    IAsyncEnumerable<Pool> GetAll();

    Task<Guid> Add(Pool pool);

    Task Remove(Pool pool);
}
