using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IWalletRepository : IRepository
{
    public IAsyncEnumerable<Wallet> GetAll();

    public Task Add(Wallet cryptocurrency);

    public Task Remove(Guid id);
}