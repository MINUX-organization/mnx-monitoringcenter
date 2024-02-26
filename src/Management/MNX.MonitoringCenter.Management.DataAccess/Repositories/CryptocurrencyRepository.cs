using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

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

    public async Task<Cryptocurrency?> GetByFullName(string fullName)
    {
        return await _context.Cryptocurrencies
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.FullName == fullName)
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string fullName, string? shortName = null)
    {
        return await _context.Cryptocurrencies
                             .AsNoTracking()
                             .AnyAsync(x => x.FullName.Equals(fullName) || x.ShortName.Equals(shortName))
                             .ConfigureAwait(false);
    }
    
    public async Task Add(Cryptocurrency cryptocurrency)
    {
        await _context.Cryptocurrencies.AddAsync(cryptocurrency).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Remove(Cryptocurrency cryptocurrency)
    {
        // TODO: добавить удаление всех связанных с монетой объектов

        _context.Cryptocurrencies.Remove(cryptocurrency);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
