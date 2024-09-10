using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётных листов.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetsQuery(Guid UserId) : IStreamRequest<FlightSheetModelBase>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetsQuery"/>.
/// </summary>
public class GetFlightSheetsQueryHandler : IStreamRequestHandler<GetFlightSheetsQuery, FlightSheetModelBase>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _repository;

    public GetFlightSheetsQueryHandler(IMapper mapper, IFlightSheetRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<FlightSheetModelBase> Handle(GetFlightSheetsQuery request,
                                                               [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var flightSheet in _repository.GetAllAvailable(request.UserId))
        {
            yield return _mapper.Map<FlightSheetModelBase>(flightSheet);
        }
    }
}

