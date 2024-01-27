using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.Wallets.RemoveWallet;

/// <summary>
/// Обработчик команды удаления кошелька
/// </summary>
public class RemoveWalletCommandHandler : IRequestHandler<RemoveWalletCommand, Result<Unit>>
{
    private readonly IWalletRepository _repository;

    public RemoveWalletCommandHandler(IWalletRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<Unit>> Handle(RemoveWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _repository.GetById(request.Id);

        if (wallet == null)
        {
            return Result<Unit>.Invalid("Wallet with this Id wasn't found");
        }

        await _repository.Remove(wallet);
        return Result<Unit>.Success(Unit.Value);
    }
}
