using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

/// <summary>
/// Обработчик команды добавления кошелька
/// </summary>
public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, Result<WalletModel>>
{
    private readonly IWalletRepository _walletRepository;

    private readonly ICryptocurrencyRepository _cryptocurrencyRepository;

    private readonly IMapper _mapper;

    public AddWalletCommandHandler(IWalletRepository walletRepository,
                                   ICryptocurrencyRepository cryptocurrencyRepository,
                                   IMapper mapper)
    {
        _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
        _cryptocurrencyRepository = cryptocurrencyRepository ?? throw new ArgumentNullException(nameof(cryptocurrencyRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<WalletModel>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        if (await _walletRepository.Exists(request.Model.Name, request.Model.Address))
        {
            return Result<WalletModel>.Invalid("Wallet already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository.GetById(request.Model.CryptocurrencyId);

        if (cryptocurrency is null)
        {
            return Result<WalletModel>.Invalid("Cryptocurrency wasn't found");
        }

        var wallet = _mapper.Map<Wallet>(request.Model);
        wallet.Id = await _walletRepository.Add(wallet).ConfigureAwait(false);
        wallet.Cryptocurrency = cryptocurrency;

        return Result<WalletModel>.SuccessfullyCreated(_mapper.Map<WalletModel>(wallet));
    }
}