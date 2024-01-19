using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

public interface IPoolRepository
{
    IAsyncEnumerable<Pool> GetAll();

    Task<Guid> Add(Pool pool);

    Task Remove(Pool pool);
}
