using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.DataAccess.Repositories;

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
        await _context.Wallets.AddAsync(wallet);
        return wallet.Id;
    }

    public void Remove(Wallet wallet)
    {
        _context.Wallets.Remove(wallet);
    }
}
