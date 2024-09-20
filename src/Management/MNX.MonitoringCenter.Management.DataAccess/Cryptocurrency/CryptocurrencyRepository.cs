using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;

using Cryptocurrency = Core.Cryptocurrency;

/// <summary>
/// Реализация <see cref="ICryptocurrencyRepository"/>.
/// </summary>
public class CryptocurrencyRepository : ICryptocurrencyRepository
{
    private readonly Context _context;

    public CryptocurrencyRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Cryptocurrency> GetAllAvailable(Guid userId)
    {
        return _context.Cryptocurrencies.AsNoTrackingWithIdentityResolution()
                                        .Where(x => x.UserId == userId)
                                        .Include(x => x.Algorithm)
                                        .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Cryptocurrency?> GetAvailableById(Guid id, Guid userId)
    {
        return _context.Cryptocurrencies.AsNoTrackingWithIdentityResolution()
                                        .Where(x => x.UserId == userId)
                                        .Include(x => x.Algorithm)
                                        .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId, string fullName, string shortName)
    {
        return _context.Cryptocurrencies
                            .AsNoTracking()
                            .Where(x => x.UserId == userId)
                            .AnyAsync(x => x.FullName.Equals(fullName) || x.ShortName.Equals(shortName));
    }

    /// <inheritdoc/>
    public async Task Add(Cryptocurrency cryptocurrency)
    {
        await _context.Cryptocurrencies.AddAsync(cryptocurrency);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId)
    {
        return _context.Cryptocurrencies.Where(x => x.Id == id && x.UserId == userId).ExecuteDeleteAsync();
    }
}
