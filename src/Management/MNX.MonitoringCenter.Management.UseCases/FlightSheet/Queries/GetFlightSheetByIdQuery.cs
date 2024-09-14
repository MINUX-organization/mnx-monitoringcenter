using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Contracts.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Queries;

/// <summary>
/// Запрос на получение полётного листа по идентификатору.
/// </summary>
/// <param name="Id"> Идентификатор. </param>
/// <param name="UserId"> Идентификатор пользователя. </param>
public sealed record GetFlightSheetByIdQuery(Guid Id, Guid UserId) : IRequest<Result<FlightSheetModelBase>>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetByIdQuery"/>.
/// </summary>
public class GetFLightSheetByIdQueryHandler : IRequestHandler<GetFlightSheetByIdQuery, Result<FlightSheetModelBase>>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _repository;

    public GetFLightSheetByIdQueryHandler(IMapper mapper, IFlightSheetRepository repository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result<FlightSheetModelBase>> Handle(GetFlightSheetByIdQuery request, CancellationToken cancellationToken)
    {
        var flightSheet = await _repository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<FlightSheetModelBase>.Invalid("Flight sheet wasn`t found!");
        }

        return Result<FlightSheetModelBase>.Success(_mapper.Map<FlightSheetModelBase>(flightSheet));
    }
}
