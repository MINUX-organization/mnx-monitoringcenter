using MINUX.Backend.Unit.Core;

namespace MINUX.Backend.Unit.UseCases.Abstractions;

public interface IWalletRepository
{
    public IAsyncEnumerable<Wallet> GetAll();

    public Task<Guid> Add(Wallet wallet);

    public Task Remove(Wallet wallet);
}