using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.DataAccess.Repositories;

/// <summary>
/// Реализация <see cref="IWalletRepository"/>
/// </summary>
public class WalletRepository : IWalletRepository
{
    private readonly Context _context;

    public WalletRepository(Context context)
    {
        _context = context;
    }

    /// <inheritdoc/>
    public IAsyncEnumerable<Wallet> GetAll()
    {
        return _context.Wallets.Include(x => x.Cryptocurrency)
                               .AsNoTracking()
                               .AsAsyncEnumerable();
    }

    /// <inheritdoc/>
    public async Task<Wallet?> GetById(Guid Id)
    {
        return await _context.Wallets
                             .Include(x => x.Cryptocurrency)
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == Id)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(string name, string address)
    {
        return await _context.Wallets
                             .AsNoTracking()
                             .AnyAsync(x => x.Name == name || x.Address == address)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<bool> Exists(string name, string address, Guid exceptId)
    {
        return await _context.Wallets
                             .AsNoTracking()
                             .AnyAsync(x => (x.Name == name || x.Address == address) && x.Id != exceptId)
                             .ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task<Guid> Add(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return wallet.Id;
    }

    /// <inheritdoc/>
    public async Task Update(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    /// <inheritdoc/>
    public async Task Remove(Wallet wallet)
    {
        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
