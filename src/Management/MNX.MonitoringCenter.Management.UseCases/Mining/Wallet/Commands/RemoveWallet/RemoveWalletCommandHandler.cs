using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.RemoveWallet;

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
        await _repository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
