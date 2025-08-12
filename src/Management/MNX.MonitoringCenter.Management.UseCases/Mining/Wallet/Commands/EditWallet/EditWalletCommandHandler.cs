using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.EditWallet;
/// <summary>
/// Обработчик команды редактирования кошелька
/// </summary>
public class EditWalletCommandHandler : IRequestHandler<EditWalletCommand, Result<WalletModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly IWalletMapper _walletMapper;

    ///
    public EditWalletCommandHandler(IWalletRepository walletRepository, IWalletMapper walletMapper)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        _walletMapper = walletMapper ?? throw new ArgumentNullException(nameof(walletMapper));
    }

    ///
    public async Task<Result<WalletModel>> Handle(EditWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (wallet is null)
        {
            return Result<WalletModel>.Invalid("Wallet with this Id wasn't found");
        }

        if (wallet.CryptocurrencyId != request.Model.CryptocurrencyId)
        {
            return Result<WalletModel>.Invalid("You cannot change the cryptocurrency");
        }

        var newWallet = _walletMapper.MapToCoreEntity(request);

        if (wallet.Equals(newWallet))
        {
            return Result<WalletModel>.Success(_walletMapper.MapToModel(wallet));
        }

        if (wallet.Name != newWallet.Name &&
            await _walletRepository.ExistsWithName(request.UserId, newWallet.Name, cancellationToken))
        {
            return Result<WalletModel>.Conflict($"Wallet with name is equaled {newWallet.Name} already exists");
        }

        await _walletRepository.Update(newWallet);
        newWallet.Cryptocurrency = wallet.Cryptocurrency;

        return Result<WalletModel>.Success(_walletMapper.MapToModel(newWallet));
    }
}
