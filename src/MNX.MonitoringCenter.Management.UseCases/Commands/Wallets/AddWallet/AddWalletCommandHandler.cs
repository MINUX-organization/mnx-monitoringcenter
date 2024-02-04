using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Commands.Wallets.AddWallet;

/// <summary>
/// Обработчик команды добавления кошелька
/// </summary>
public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, Result<Guid>>
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

    public async Task<Result<Guid>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        if (await _walletRepository.Exists(request.Model.Name, request.Model.Address))
        {
            return Result<Guid>.Invalid("Wallet already exists");
        }

        if (! await _cryptocurrencyRepository.Exists(request.Model.CryptocurrencyFullName))
        {
            return Result<Guid>.Invalid("Cryptocurrency wasn't found");
        }

        var id = await _walletRepository.Add(_mapper.Map<Wallet>(request.Model));

        return Result<Guid>.SuccessfullyCreated(id);
    }
}