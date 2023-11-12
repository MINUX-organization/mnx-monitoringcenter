using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetWalletsQuery;

public class GetWalletsQueryHandler : IStreamRequestHandler<GetWalletsQuery, Wallet>
{
    private readonly IMainRepository _repository;

    public GetWalletsQueryHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public IAsyncEnumerable<Wallet> Handle(GetWalletsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Wallets.GetAll();
    }
}