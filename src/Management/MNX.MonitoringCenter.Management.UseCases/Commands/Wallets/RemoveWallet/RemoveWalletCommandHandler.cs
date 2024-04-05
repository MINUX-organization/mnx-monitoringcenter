using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.RemoveWallet;

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
        var wallet = await _repository.GetAvailableById(request.Id, request.UserId);

        if (wallet != null)
        {
            await _repository.Remove(wallet);
        }

        return Result<Unit>.Empty();
    }
}
