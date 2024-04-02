using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.EditWallet;

/// <summary>
/// Обработчик команды редактирования кошелька
/// </summary>
public class EditWalletCommandHandler : IRequestHandler<EditWalletCommand, Result<WalletModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IMapper _mapper;

    public EditWalletCommandHandler(IWalletRepository walletRepository,
                                    ICryptocurrencyRepository cryptocurrencyRepository,
                                    IMapper mapper)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<WalletModel>> Handle(EditWalletCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetAvailableById(request.Id, request.UserId);

        if (wallet == null)
        {
            return Result<WalletModel>.Invalid("Wallet with this Id wasn't found");
        }

        if (WalletsIsEquals(wallet, request.Model))
        {
            return wallet.CryptocurrencyId == request.Model.CryptocurrencyId
                ? Result<WalletModel>.Success(_mapper.Map<WalletModel>(wallet))
                : Result<WalletModel>.Invalid("You can't change only the cryptocurrency");
        }

        if (await _walletRepository.Exists(request.UserId, request.Model.Name, request.Model.Address, request.Id))
        {
            return Result<WalletModel>.Invalid("Wallet with this name or address already exists");
        }

        var cryptocurency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId);

        if (cryptocurency is null)
        {
            return Result<WalletModel>.Invalid("Cryptocurrency wasn't found");
        }

        var newWallet = _mapper.Map<Wallet>(request);
        await _walletRepository.Update(newWallet).ConfigureAwait(false);
        newWallet.Cryptocurrency = cryptocurency;

        return Result<WalletModel>.Success(_mapper.Map<WalletModel>(newWallet));
    }

    /// <summary>
    /// Получить признак равенства моделей кошелька
    /// </summary>
    /// <param name="currentWallet"> Текущий кошелёк </param>
    /// <param name="newWallet"> Новый кошелёк </param>
    /// <returns> <see langword="true"/>, если данные моделей равны, иначе <see langword="false"/> </returns>
    private static bool WalletsIsEquals(Wallet currentWallet, WalletInputModel newWallet)
    {
        return currentWallet.Name == newWallet.Name &&
               currentWallet.Address == newWallet.Address;
    }
}
