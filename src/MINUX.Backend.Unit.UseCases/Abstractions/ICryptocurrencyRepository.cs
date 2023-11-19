using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface ICryptocurrencyRepository
{
    IAsyncEnumerable<Cryptocurrency> GetAll();

    Task<bool> Exists(Guid id);

    Task<bool> Exists(string shortName, string fullName);

    Task<Guid> Add(Cryptocurrency cryptocurrency);

    Task Remove(Cryptocurrency cryptocurrency);
}