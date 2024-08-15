using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;
using System.Collections.Immutable;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

public class CryptocurrencyRepository : ICryptocurrencyRepository
{
    private readonly Context _context;

    public CryptocurrencyRepository(Context context)
    {
        _context = context;
    }

    public IAsyncEnumerable<Cryptocurrency> GetAllAvailable(long userId)
    {
        return _context.Cryptocurrencies
                       .Where(x => x.UserId == userId)
                       .AsNoTracking()
                       .AsAsyncEnumerable();
    }

    public async Task<Cryptocurrency?> GetAvailableById(Guid id, long userId)
    {
        return await _context.Cryptocurrencies
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
                             .FirstOrDefaultAsync(x => x.Id == id)
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(long userId, string fullName, string shortName)
    {
        return await _context.Cryptocurrencies
                             .AsNoTracking()
                             .Where(x => x.UserId == userId)
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
    