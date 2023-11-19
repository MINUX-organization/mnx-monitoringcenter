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
        return await _context.Cryptocurrencies
                             .AnyAsync(x => x.Id.Equals(id))
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string shortName, string fullName)
    {
        return await _context.Cryptocurrencies
                             .AnyAsync(x => x.ShortName.Equals(shortName) || x.FullName.Equals(fullName))
                             .ConfigureAwait(false);
    }
    
    public async Task<Guid> Add(Cryptocurrency cryptocurrency)
    {
        await _context.Cryptocurrencies.AddAsync(cryptocurrency).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return cryptocurrency.Id;
    }

    public async Task Remove(Cryptocurrency cryptocurrency)
    {
        _context.Cryptocurrencies.Remove(cryptocurrency);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
