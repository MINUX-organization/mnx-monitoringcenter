using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Commands.AddWalletCommand;

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
        if (!await _cryptocurrencyRepository.Exists(request.CryptocurrencyId))
        {
            return Result<Guid>.NotFound("Cryptocurrency wasn't found");
        }

        var id = await _walletRepository.Add(_mapper.Map<Wallet>(request));

        return Result<Guid>.SuccessfullyCreated(id);
    }
}