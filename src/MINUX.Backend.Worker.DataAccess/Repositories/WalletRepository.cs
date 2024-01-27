using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.DataAccess.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly Context _context;

    public WalletRepository(Context context)
    {
        _context = context;
    }

    public IAsyncEnumerable<Wallet> GetAll()
    {
        return _context.Wallets.AsNoTracking().AsAsyncEnumerable();
    }

    public async Task<Wallet?> GetById(Guid Id)
    {
        return await _context.Wallets
                             .AsNoTracking()
                             .FirstOrDefaultAsync(x => x.Id == Id)
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string name, string address)
    {
        return await _context.Wallets
                             .AsNoTracking()
                             .AnyAsync(x => x.Name == name || x.Address == address)
                             .ConfigureAwait(false);
    }

    public async Task<bool> Exists(string name, string address, Guid exceptId)
    {
        return await _context.Wallets
                             .AsNoTracking()
                             .AnyAsync(x => (x.Name == name || x.Address == address) && x.Id != exceptId)
                             .ConfigureAwait(false);
    }

    public async Task<Guid> Add(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return wallet.Id;
    }

    public async Task Update(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }

    public async Task Remove(Wallet wallet)
    {
        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
