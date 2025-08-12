using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.CreateFlightSheet;

/// <summary>
/// Обработчик команды добавления полётного листа.
/// </summary>
public class CreateFlightSheetCommandHandler :
    IRequestHandler<CreateFlightSheetCommand, Result<Guid>>
{
    private readonly IFlightSheetMapper _flightSheetMapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    ///
    public CreateFlightSheetCommandHandler(IFlightSheetMapper flightSheetMapper,
                                           IFlightSheetRepository flightSheetRepository)
    {
        _flightSheetMapper = flightSheetMapper ?? throw new ArgumentNullException(nameof(flightSheetMapper));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));
    }

    ///
    public async Task<Result<Guid>> Handle(CreateFlightSheetCommand request,
                                           CancellationToken cancellationToken)
    {
        if (await _flightSheetRepository.ExistsAvailable(request.Model.Name, request.UserId, cancellationToken))
        {
            return Result<Guid>
                .Invalid($"Flight sheet with name {request.Model.Name} already exist!");
        }

        var flightSheet = _flightSheetMapper.MapToCoreEntity(request.Model, request.UserId);

        await _flightSheetRepository.Add(flightSheet);

        return Result<Guid>.SuccessfullyCreated(flightSheet.Id);
    }
}
