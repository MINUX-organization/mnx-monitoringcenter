using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

public class CryptocurrencyRepository : ICryptocurrencyRepository
{
    private readonly Context _context;

    public CryptocurrencyRepository(Context context)
    {
        _context = context;
    }

    public Task Add(Cryptocurrency cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Cryptocurrency> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Remove(string shortName)
    {
        throw new NotImplementedException();
    }
}
