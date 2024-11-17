using AutoMapper;
using MediatR;
using System.Runtime.CompilerServices;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
public sealed record GetFlightSheetsQuery : IStreamRequest<FlightSheetModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetFlightSheetsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

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
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IMapper mapper, IFlightSheetRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<FlightSheetModel> Handle(GetFlightSheetsQuery request,
                                                          [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var flightSheets = _repository.GetAllAvailable(request.Specification);

        await foreach (var flightSheet in flightSheets.WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<FlightSheetModel>(flightSheet);
        }
    }
}

