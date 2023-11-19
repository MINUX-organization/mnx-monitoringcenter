using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetWalletsQuery;

public class GetWalletsQueryHandler : IStreamRequestHandler<GetWalletsQuery, Wallet>
{
    private readonly IWalletRepository _repository;

    public GetWalletsQueryHandler(IWalletRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Wallet> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll();
    }
}