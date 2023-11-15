using AutoMapper;
using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Commands.AddWalletCommand;

/// <summary>
/// Обработчик добавления кошелька
/// </summary>
public class AddWalletCommandHandler : IRequestHandler<AddWalletCommand, Result<Guid>>
{
    private readonly IMainRepository _repository;

    private readonly IMapper _mapper;

    public AddWalletCommandHandler(IMainRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<Guid>> Handle(AddWalletCommand request, CancellationToken cancellationToken)
    {
        if (!await _repository.Cryptocurrencies.Exists(request.CryptocurrencyId))
        {
            return Result<Guid>.NotFound("Cryptocurrency wasn't found");
        }

        var id = await _repository.Wallets.Add(_mapper.Map<Wallet>(request));
        await _repository.SaveChangesAsync();

        return Result<Guid>.SuccessfullyCreated(id);
    }
}