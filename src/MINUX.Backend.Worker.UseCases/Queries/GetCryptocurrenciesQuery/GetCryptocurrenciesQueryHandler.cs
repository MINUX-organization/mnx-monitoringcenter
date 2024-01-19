using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.GetCryptocurrenciesQuery;

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
