using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Cryptocurrency;

namespace MNX.MonitoringCenter.Management.UseCases.Wallet.Commands.AddWallet;

using Wallet = Core.Wallet;

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
        if (await _walletRepository.ExistsWithName(request.UserId, request.Model.Name))
        {
            return Result<WalletModel>.Conflict($"Wallet with name is equaled {request.Model.Name} already exists");
        }

        if (await _walletRepository.ExistsWithAddress(request.UserId, request.Model.Address))
        {
            return Result<WalletModel>.Conflict($"Wallet with address is equaled {request.Model.Address} already exists");
        }

        var cryptocurrency = await _cryptocurrencyRepository
            .GetAvailableById(request.Model.CryptocurrencyId, request.UserId);

        if (cryptocurrency is null)
        {
            return Result<WalletModel>.Invalid("Cryptocurrency wasn't found");
        }

        var wallet = _mapper.Map<Wallet>(request);
        await _walletRepository.Add(wallet);
        wallet.Cryptocurrency = cryptocurrency;

        return Result<WalletModel>.SuccessfullyCreated(_mapper.Map<WalletModel>(wallet));
    }
}