using MINUX.Backend.Worker.Core;

namespace MINUX.Backend.Worker.UseCases.Abstractions;

public interface IWalletRepository
{
    public IAsyncEnumerable<Wallet> GetAll();

    public Task<Guid> Add(Wallet wallet);

    public Task Remove(Wallet wallet);
}