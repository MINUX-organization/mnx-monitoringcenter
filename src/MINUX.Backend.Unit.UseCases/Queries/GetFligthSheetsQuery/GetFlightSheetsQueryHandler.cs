using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheet>
{
    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IFlightSheetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<FlightSheet> Handle(GetFlightSheetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll();
    }
}
