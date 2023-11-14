using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface ICryptocurrencyRepository : IRepository
{
    public IAsyncEnumerable<Cryptocurrency> GetAll();

    public Task Add(Cryptocurrency cryptocurrency);

    public Task Remove(string shortName);
}