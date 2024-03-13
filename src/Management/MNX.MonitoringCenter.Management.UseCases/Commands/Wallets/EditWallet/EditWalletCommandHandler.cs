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
        var wallet = await _walletRepository.GetById(request.Id);

        // TODO: утвердить валидацию исходя из бизнес тербований

        if (wallet == null)
        {
            return Result<WalletModel>.Invalid("Wallet with this Id wasn't found");
        }

        if (WalletsIsEquals(wallet, request.Model))
        {
            return Result<WalletModel>.Success(_mapper.Map<WalletModel>(wallet));
        }

        if (await _walletRepository.Exists(request.Model.Name, request.Model.Address, request.Id))
        {
            return Result<WalletModel>.Invalid("Wallet with this name or address already exists");
        }

        if ((await _cryptocurrencyRepository.GetById(request.Model.CryptocurrencyId)) is null)
        {
            return Result<WalletModel>.Invalid("Cryptocurrency wasn't found");
        }

        var newWallet = _mapper.Map<Wallet>(request.Model);
        newWallet.Id = request.Id;
        await _walletRepository.Update(newWallet);
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
               currentWallet.Address == newWallet.Address &&
               currentWallet.Cryptocurrency?.Id == newWallet.CryptocurrencyId;
    }
}
