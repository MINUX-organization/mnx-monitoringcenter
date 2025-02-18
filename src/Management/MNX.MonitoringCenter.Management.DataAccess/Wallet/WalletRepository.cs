using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using MNX.MonitoringCenter.Management.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Mining.Wallet;

namespace MNX.MonitoringCenter.Management.DataAccess.Wallet;

using Wallet = Core.Mining.Wallet;

/// <summary>
/// Реализация <see cref="IWalletRepository"/>
/// </summary>
public class WalletRepository : IWalletRepository
{
    private readonly Context _context;

    public WalletRepository(Context context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Wallet> GetAllAvailable(Specification specification)
    {
        return _context.Wallets.Include(x => x.Cryptocurrency)
                               .Where(x => x.UserId == specification.UserId)
                               .Filter(specification)
                               .AsNoTrackingWithIdentityResolution()
                               .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Wallet?> GetAvailableById(Guid id, Guid userId, CancellationToken cancellationToken)
    {
        return _context.Wallets.Include(x => x.Cryptocurrency)
                                    .ThenInclude(c => c!.Algorithm)
                               .AsNoTrackingWithIdentityResolution()
                               .Where(x => x.UserId == userId)
                               .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsWithName(Guid userId, string name, CancellationToken cancellationToken)
    {
        return Exists(userId, x => x.Name == name, cancellationToken);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsWithAddress(Guid userId, string address, CancellationToken cancellationToken)
    {
        return Exists(userId, x => x.Address == address, cancellationToken);
    }

    /// <inheritdoc/>
    public async Task Add(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Update(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        return _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public Task Remove(Guid id, Guid userId)
    {
        return _context.Wallets
            .Where(x => x.Id == id && x.UserId == userId)
            .ExecuteDeleteAsync();
    }

    /// <summary>
    /// Получить признак существования кошелька по переданной лямбде.
    /// </summary>
    /// <param name="userId"> Идентификатор пользователя. </param>
    /// <param name="expression"> Выражение, по которому должен производиться поиск. </param>
    /// <param name="cancellationToken"> Токен отмены. </param>
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    private Task<bool> Exists(Guid userId, Expression<Func<Wallet, bool>> expression, CancellationToken cancellationToken)
    {
        return _context.Wallets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .AnyAsync(expression, cancellationToken);
    }
}
