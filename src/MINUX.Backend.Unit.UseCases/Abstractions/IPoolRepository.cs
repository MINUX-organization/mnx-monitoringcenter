using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IPoolRepository
{
    public IAsyncEnumerable<Pool> GetAll();

    public Task Add(Pool cryptocurrency);

    public Task Remove(Guid id);
}
