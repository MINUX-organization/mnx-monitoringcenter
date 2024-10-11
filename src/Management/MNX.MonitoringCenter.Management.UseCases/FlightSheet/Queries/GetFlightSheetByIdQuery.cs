using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries.Models;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётного листа по идентификатору.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetByIdQuery(Guid Id, Guid UserId) : IRequest<Result<FlightSheetOutputModel>>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetByIdQuery"/>.
/// </summary>
public class GetFLightSheetByIdQueryHandler : IRequestHandler<GetFlightSheetByIdQuery, Result<FlightSheetOutputModel>>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _repository;

    public GetFLightSheetByIdQueryHandler(IMapper mapper, IFlightSheetRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<FlightSheetOutputModel>> Handle(GetFlightSheetByIdQuery request, CancellationToken cancellationToken)
    {
        var flightSheet = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<FlightSheetOutputModel>.Invalid("Flight sheet was not found!");
        }

        return Result<FlightSheetOutputModel>.Success(_mapper.Map<FlightSheetOutputModel>(flightSheet));
    }
}
