using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetCryptocurrenciesQueryHandler : IStreamRequestHandler<GetCryptocurrenciesQuery, Cryptocurrency>
{
    private readonly ICryptocurrencyRepository _repository;

    public GetCryptocurrenciesQueryHandler(ICryptocurrencyRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Cryptocurrency> Handle(GetCryptocurrenciesQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll();
    }
}
