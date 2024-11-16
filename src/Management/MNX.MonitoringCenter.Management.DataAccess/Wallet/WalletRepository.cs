using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.UseCases.Wallet;
using System.Linq.Expressions;

namespace MNX.MonitoringCenter.Management.DataAccess.Wallet;

using Wallet = Core.Wallet;

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
    public IAsyncEnumerable<Wallet> GetAllAvailable(Guid userId)
    {
        return _context.Wallets.Include(x => x.Cryptocurrency)
                               .Where(x => x.UserId == userId)
                               .AsNoTrackingWithIdentityResolution()
                               .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public Task<Wallet?> GetAvailableById(Guid Id, Guid userId)
    {
        return _context.Wallets.Include(x => x.Cryptocurrency)
                                    .ThenInclude(c => c!.Algorithm)
                               .AsNoTrackingWithIdentityResolution()
                               .Where(x => x.UserId == userId)
                               .FirstOrDefaultAsync(x => x.Id == Id);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsWithName(Guid userId, string name)
    {
        return Exists(userId, x => x.Name == name);
    }

    /// <inheritdoc/>
    public Task<bool> ExistsWithAddress(Guid userId, string address)
    {
        return Exists(userId, x => x.Address == address);
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
    /// <returns>
    /// <see langword="true"/>, если существует, иначе <see langword="false"/>.
    /// </returns>
    private Task<bool> Exists(Guid userId, Expression<Func<Wallet, bool>> expression)
    {
        return _context.Wallets.AsNoTracking()
                               .Where(x => x.UserId == userId)
                               .AnyAsync(expression);
    }
}
