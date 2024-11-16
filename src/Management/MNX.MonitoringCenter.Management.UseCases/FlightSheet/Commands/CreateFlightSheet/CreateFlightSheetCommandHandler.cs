using AutoMapper;
using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.CreateFlightSheet;

/// <summary>
/// Обработчик команды добавления полётного листа.
/// </summary>
public class CreateFlightSheetCommandHandler :
    IRequestHandler<CreateFlightSheetCommand, Result<Guid>>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    public CreateFlightSheetCommandHandler(IMapper mapper,
                                           IFlightSheetRepository flightSheetRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));
    }

    public async Task<Result<Guid>> Handle(CreateFlightSheetCommand request,
                                           CancellationToken cancellationToken)
    {
        var flightSheet = _mapper.Map<Core.FlightSheet.FlightSheet>(request.Model);
        flightSheet.UserId = request.UserId;

        if (await _flightSheetRepository.ExistsAvailable(flightSheet.Name, request.UserId, cancellationToken))
        {
            return Result<Guid>
                .Invalid($"Flight sheet with name {flightSheet.Name} already exist!");
        }

        await _flightSheetRepository.Add(flightSheet);

        return Result<Guid>.SuccessfullyCreated(flightSheet.Id);
    }
}
