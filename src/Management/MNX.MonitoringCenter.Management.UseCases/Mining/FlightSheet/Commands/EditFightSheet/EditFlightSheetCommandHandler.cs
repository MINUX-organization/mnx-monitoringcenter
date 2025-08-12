using MediatR;
using MNX.Application.UseCases.Results;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet.Commands.EditFightSheet;

/// <summary>
/// Обработчик команды обновления полётного листа.
/// </summary>
public class EditFlightSheetCommandHandler : IRequestHandler<EditFlightSheetCommand, Result<Unit>>
{
    private readonly IFlightSheetMapper _flightSheetMapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    ///
    public EditFlightSheetCommandHandler(IFlightSheetMapper flightSheetMapper,
                                         IFlightSheetRepository flightSheetRepository)
    {
        _flightSheetMapper = flightSheetMapper ?? throw new ArgumentNullException(nameof(flightSheetMapper));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));
    }

    ///
    public async Task<Result<Unit>> Handle(EditFlightSheetCommand request, CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<Unit>.Invalid($"Flight sheet with id equaled {request.Id} was not found!");
        }

        var newFlightSheet = _flightSheetMapper.MapToCoreEntity(request);

        if (flightSheet.Name != newFlightSheet.Name &&
            await _flightSheetRepository.ExistsAvailable(newFlightSheet.Name, request.UserId, cancellationToken))
        {
            return Result<Unit>.Invalid($"Flight sheet with name {newFlightSheet.Name} already exist!");
        }

        await _flightSheetRepository.Edit(newFlightSheet);

        return Result<Unit>.Empty();
    }
}
