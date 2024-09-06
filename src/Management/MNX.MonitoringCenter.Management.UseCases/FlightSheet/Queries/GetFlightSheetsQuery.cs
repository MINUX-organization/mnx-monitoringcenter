using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
public class GetFlightSheetsQuery : IStreamRequest<Core.FlightSheet>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; set; }

    public GetFlightSheetsQuery(Guid userId)
    {
        UserId = userId;
    }
}

/// <summary>
/// Обработчик запроса на получения полётных листов.
/// </summary>
public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, Core.FlightSheet>
{
    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IFlightSheetRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Core.FlightSheet> Handle(GetFlightSheetsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAllAvailable(request.UserId);
    }
}

