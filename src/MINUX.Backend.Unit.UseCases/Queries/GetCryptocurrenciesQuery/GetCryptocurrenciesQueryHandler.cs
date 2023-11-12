using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQueryHandler : IStreamRequestHandler<GetCryptocurrenciesQuery, Cryptocurrency>
{
    private readonly IMainRepository _repository;

    public GetCryptocurrenciesQueryHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public IAsyncEnumerable<Cryptocurrency> Handle(GetCryptocurrenciesQuery request, CancellationToken cancellationToken)
    {
        return _repository.Cryptocurrencies.GetAll();
    }
}
