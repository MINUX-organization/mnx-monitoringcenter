using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries.Models;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetsQuery(Guid UserId) : IStreamRequest<FlightSheetOutputModel>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetsQuery"/>.
/// </summary>
public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheetOutputModel>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IMapper mapper, IFlightSheetRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<FlightSheetOutputModel> Handle(GetFlightSheetsQuery request,
                                                          [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var flightSheet in _repository.GetAllAvailable(request.UserId).WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<FlightSheetOutputModel>(flightSheet);
        }
    }
}

