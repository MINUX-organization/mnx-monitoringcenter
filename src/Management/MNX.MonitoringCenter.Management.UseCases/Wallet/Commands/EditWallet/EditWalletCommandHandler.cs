using AutoMapper;
using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.EditWallet;

using Wallet = Core.Wallet;

/// <summary>
/// Обработчик команды редактирования кошелька
/// </summary>
public class EditWalletCommandHandler : IRequestHandler<EditWalletCommand, Result<WalletModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly IMapper _mapper;

    public EditWalletCommandHandler(IWalletRepository walletRepository, IMapper mapper)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

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

        var newWallet = _mapper.Map<Wallet>(request);

        if (wallet.Equals(newWallet))
        {
            return Result<WalletModel>.Success(_mapper.Map<WalletModel>(wallet));
        }

        if (wallet.Name != newWallet.Name &&
            await _walletRepository.ExistsWithName(request.UserId, newWallet.Name, cancellationToken))
        {
            return Result<WalletModel>.Conflict($"Wallet with name is equaled {newWallet.Name} already exists");
        }

        if (wallet.Address != newWallet.Address &&
            await _walletRepository.ExistsWithAddress(request.UserId, newWallet.Address, cancellationToken))
        {
            return Result<WalletModel>.Conflict($"Wallet with address is equaled {newWallet.Address} already exists");
        }

        await _walletRepository.Update(newWallet);
        newWallet.Cryptocurrency = wallet.Cryptocurrency;

        return Result<WalletModel>.Success(_mapper.Map<WalletModel>(newWallet));
    }
}
