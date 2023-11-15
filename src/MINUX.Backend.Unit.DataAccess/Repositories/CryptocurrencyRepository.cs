using Microsoft.EntityFrameworkCore;
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

    public IAsyncEnumerable<Cryptocurrency> GetAll()
    {
        return _context.Cryptocurrencies
                       .AsNoTracking()
                       .Include(x => x.Wallets)
                       .Include(x => x.Pools)
                       .AsAsyncEnumerable();
    }

    public async Task<bool> Exists(Guid id)
    {
        return ! (await _context.Cryptocurrencies.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id) == null);
    }

    public async Task<bool> Exists(string shortName, string fullName)
    {
        return ! (await _context.Cryptocurrencies
                                .AsNoTracking()
                                .FirstOrDefaultAsync(x => x.ShortName == shortName || x.FullName == fullName) == null);
    }
    
    public async Task<Guid> Add(Cryptocurrency cryptocurrency)
    {
        await _context.Cryptocurrencies.AddAsync(cryptocurrency);
        return cryptocurrency.Id;
    }

    public void Remove(Cryptocurrency cryptocurrency)
    {
        _context.Cryptocurrencies.Remove(cryptocurrency);
    }
}
