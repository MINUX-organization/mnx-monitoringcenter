using MediatR;
using MNX.Application.UseCases.Results;
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
        await _flightSheetRepository.Remove(request.Id, request.UserId);
        return Result<Unit>.Empty();
    }
}
