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

    public async Task<Guid> Add(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet).ConfigureAwait(false);
        await _context.SaveChangesAsync().ConfigureAwait(false);

        return wallet.Id;
    }

    public async Task Remove(Wallet wallet)
    {
        _context.Wallets.Remove(wallet);
        await _context.SaveChangesAsync().ConfigureAwait(false);
    }
}
