using AutoMapper;
using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.EditFightSheet;

/// <summary>
/// Обработчик команды обновления полётного листа.
/// </summary>
public class EditFlightSheetCommandHandler : IRequestHandler<EditFlightSheetCommand, Result<Unit>>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    public EditFlightSheetCommandHandler(IMapper mapper,
                                         IFlightSheetRepository flightSheetRepository)
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
            return Result<Unit>.Invalid($"Flight sheet with id equaled {request.Id} was not found!");
        }

        var newFlightSheet = _mapper.Map<Core.FlightSheet.FlightSheet>(request.Model);
        newFlightSheet.Id = request.Id;
        newFlightSheet.UserId = request.UserId;
        newFlightSheet.Targets.ForEach(x => x.FlightSheetId = newFlightSheet.Id);

        if (flightSheet.Name != newFlightSheet.Name &&
            await _flightSheetRepository.ExistsAvailable(newFlightSheet.Name, request.UserId, cancellationToken))
        {
            return Result<Unit>.Invalid($"Flight sheet with name {newFlightSheet.Name} already exist!");
        }

        await _flightSheetRepository.Edit(newFlightSheet);

        return Result<Unit>.Empty();
    }
}
