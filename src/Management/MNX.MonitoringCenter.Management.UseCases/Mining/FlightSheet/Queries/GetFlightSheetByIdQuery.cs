using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётного листа по идентификатору.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetByIdQuery(Guid Id, Guid UserId) : IRequest<Result<FlightSheetModel>>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetByIdQuery"/>.
/// </summary>
public class GetFLightSheetByIdQueryHandler : IRequestHandler<GetFlightSheetByIdQuery, Result<FlightSheetModel>>
{
    private readonly IFlightSheetMapper _flightSheetMapper;

    private readonly IFlightSheetRepository _repository;

    ///
    public GetFLightSheetByIdQueryHandler(IFlightSheetMapper flightSheetMapper, IFlightSheetRepository repository)
    {
        _flightSheetMapper = flightSheetMapper ?? throw new ArgumentNullException(nameof(flightSheetMapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    ///
    public async Task<Result<FlightSheetModel>> Handle(GetFlightSheetByIdQuery request, CancellationToken cancellationToken)
    {
        var flightSheet = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<FlightSheetModel>.Invalid($"Flight sheet with id equaled {request.Id} was not found!");
        }

        return Result<FlightSheetModel>.Success(_flightSheetMapper.MapToModel(flightSheet));
    }
}
