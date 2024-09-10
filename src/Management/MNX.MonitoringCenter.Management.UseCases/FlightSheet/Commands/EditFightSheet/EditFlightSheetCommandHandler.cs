using AutoMapper;
using MediatR;
using MNX.Application.UseCases;
using MNX.MonitoringCenter.Management.Core.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheets.Commands.EditFightSheet;

/// <summary>
/// Обработчик команды обновления полётного листа.
/// </summary>
public class EditFlightSheetCommandHandler :
    IRequestHandler<EditFlightSheetCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    public EditFlightSheetCommandHandler(IMapper mapper, IFlightSheetRepository flightSheetRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));
    }

    public async Task<Result<Unit>> Handle(EditFlightSheetCommand request, CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository.GetAvailableById(request.Id, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<Unit>.Invalid($"Flight sheet with id is equaled {request.Id} was not found!");
        }

        var newFlightSheet = _mapper.Map<FlightSheetBase>(request.Model);
        newFlightSheet.Id = request.Id;
        newFlightSheet.UserId = request.UserId;

        if (flightSheet.Name != newFlightSheet.Name &&
            await _flightSheetRepository.Exists(newFlightSheet.Name, request.UserId, cancellationToken))
        {
            return Result<Unit>.Invalid($"Flight sheet with name {newFlightSheet.Name} already exist!");
        }

        if (flightSheet.Equals(newFlightSheet))
        {
            return Result<Unit>.Empty();
        }

        if (flightSheet.Type != request.Model.Type)
        {
            return Result<Unit>.Invalid("You cannot change the type of flight sheet!");
        }

        await _flightSheetRepository.Edit(newFlightSheet, cancellationToken);

        return Result<Unit>.Empty();
    }
}
