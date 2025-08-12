using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Mining.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Wallet.Commands.AddWallet;

/// <summary>
/// Обработчик команды добавления кошелька
/// </summary>
public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, Result<WalletModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IWalletMapper _walletMapper;

    ///
    public AddWalletCommandHandler(IWalletRepository walletRepository,
                                   ICryptocurrencyRepository cryptocurrencyRepository,
                                   IWalletMapper walletMapper)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _walletMapper = walletMapper ?? throw new ArgumentNullException(nameof(walletMapper));
    }

    ///
    public async Task<Result<WalletModel>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        if (await _walletRepository.ExistsWithName(request.UserId, request.Model.Name, cancellationToken))
        {
            return Result<WalletModel>.Conflict($"Wallet with name is equaled {request.Model.Name} already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId, cancellationToken);

        if (cryptocurrency is null)
        {
            return Result<WalletModel>.Invalid("Cryptocurrency wasn't found");
        }

        var wallet = _walletMapper.MapToCoreEntity(request);
        await _walletRepository.Add(wallet);
        wallet.Cryptocurrency = cryptocurrency;

        return Result<WalletModel>.SuccessfullyCreated(_walletMapper.MapToModel(wallet));
    }
}