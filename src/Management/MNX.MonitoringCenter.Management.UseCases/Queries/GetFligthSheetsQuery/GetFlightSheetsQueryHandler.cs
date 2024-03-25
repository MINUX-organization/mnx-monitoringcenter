using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetCryptocurrenciesQuery;

public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheet>
{
    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IFlightSheetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<FlightSheet> Handle(GetFlightSheetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllAvailable(request.UserId);
    }
}
