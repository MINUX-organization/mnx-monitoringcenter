using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.RemoveFlightSheet;

namespace MNX.MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.RemoveFlightSheet;

/// <summary>
/// Обработчик удаления полётного листа.
/// </summary>
public class RemoveFlightSheetCommandHandler : IRequestHandler<RemoveFlightSheetCommand, Result<Unit>>
{
    private readonly IFlightSheetRepository _flightSheetRepository;

    public RemoveFlightSheetCommandHandler(IFlightSheetRepository flightSheetRepository)
    {
        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));
    }

    public async Task<Result<Unit>> Handle(RemoveFlightSheetCommand request, CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is not null)
        {
            await _flightSheetRepository.Remove(flightSheet, cancellationToken);
        }
        
        return Result<Unit>.Empty();
    }
}
