using AutoMapper;
using MediatR;
using System.Runtime.CompilerServices;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetsQuery(Guid UserId) : IStreamRequest<FlightSheetModel>;

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
        await foreach (var flightSheet in _repository.GetAllAvailable(request.UserId).WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<FlightSheetModel>(flightSheet);
        }
    }
}

