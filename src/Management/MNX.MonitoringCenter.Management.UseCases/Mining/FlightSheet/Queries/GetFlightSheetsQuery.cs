using MediatR;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
public sealed record GetFlightSheetsQuery : IStreamRequest<FlightSheetModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    ///
    public GetFlightSheetsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
    public GetFlightSheetsQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик <see cref="GetFlightSheetsQuery"/>.
/// </summary>
public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheetModel>
{
    private readonly IFlightSheetMapper _flightSheetMapper;

    private readonly IFlightSheetRepository _repository;

    ///
    public GetFlightSheetsQueryHandler(IFlightSheetMapper flightSheetMapper, IFlightSheetRepository repository)
    {
        _flightSheetMapper = flightSheetMapper ?? throw new ArgumentNullException(nameof(flightSheetMapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async IAsyncEnumerable<FlightSheetModel> Handle(GetFlightSheetsQuery request,
                                                          [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var flightSheets = _repository.GetAllAvailable(request.Specification);

        await foreach (var flightSheet in flightSheets.WithCancellation(cancellationToken))
        {
            yield return _flightSheetMapper.MapToModel(flightSheet);
        }
    }
}

