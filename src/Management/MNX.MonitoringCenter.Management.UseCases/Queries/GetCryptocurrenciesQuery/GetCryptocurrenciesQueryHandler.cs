using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

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
