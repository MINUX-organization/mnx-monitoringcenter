using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheet>
{
    private readonly IMainRepository _repository;

    public GetFlightSheetsQueryHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public IAsyncEnumerable<FlightSheet> Handle(GetFlightSheetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.FlightSheets.GetAll();
    }
}
