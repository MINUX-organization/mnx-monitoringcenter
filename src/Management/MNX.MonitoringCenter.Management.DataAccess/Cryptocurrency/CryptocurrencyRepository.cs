using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.DataAccess.Cryptocurrency;

using Cryptocurrency = Core.Mining.Cryptocurrency;

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
    public IAsyncEnumerable<Cryptocurrency> GetAllAvailable(Specification specification)
    {
        return _context.Cryptocurrencies.AsNoTrackingWithIdentityResolution()
                                        .Available(specification.UserId)
                                        .Filter(specification)
                                        .Sort()
                                        .Include(x => x.Algorithm)
                                        .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Cryptocurrency?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.Cryptocurrencies.AsNoTrackingWithIdentityResolution()
                                        .Available(userId)
                                        .Include(x => x.Algorithm)
                                        .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> Exists(Guid userId,
                             string fullName,
                             string shortName,
                             CancellationToken cancellationToken)
    {
        return _context.Cryptocurrencies
                            .AsNoTracking()
                            .Available(userId)
                            .AnyAsync(x => x.FullName.Equals(fullName) ||
                                           x.ShortName.Equals(shortName), cancellationToken);
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
        return _context.Cryptocurrencies
            .Where(x => x.Id == id && x.OwnerId == userId)
            .ExecuteDeleteAsync();
    }
}
