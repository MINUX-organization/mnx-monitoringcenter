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

    public Task Add(Wallet cryptocurrency)
    {
        throw new NotImplementedException();
    }

    public IAsyncEnumerable<Wallet> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task Remove(Guid id)
    {
        throw new NotImplementedException();
    }
}
