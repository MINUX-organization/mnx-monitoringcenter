using MediatR;
using MNX.Application.UseCases.CommandValidation;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Core.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetFlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.FlightSheet.Commands.ApplyFlightSheet;

/// <summary>
/// Команда применения полётного листа на майнинг устройство.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="FightSheetId"> Идентификатор полётного листа. </param>
/// <param name="MiningDevices"> Идентификаторы майнинг устройств. </param>
public sealed record ApplyFlightSheetCommand(Guid UserId, Guid FightSheetId, Guid[] MiningDevices)
    : IValidatableCommand<Guid[]>;


/// <summary>  
/// Обработчик <see cref="ApplyFlightSheetCommand"/>.
/// </summary>
public class ApplyFlightSheetCommandHandler : IRequestHandler<ApplyFlightSheetCommand, Result<Guid[]>>
{
    private readonly IMediator _mediator;

    private readonly IFlightSheetRepository _flightSheetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ApplyFlightSheetCommandHandler(IMediator mediator,
                                          IFlightSheetRepository flightSheetRepository,
                                          IMiningDeviceRepository miningDeviceRepository)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Guid[]>> Handle(ApplyFlightSheetCommand request, CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository.GetAvailableById(request.FightSheetId, request.UserId, cancellationToken);

        if (flightSheet is null)
        {
            return Result<Guid[]>.Invalid("Flight sheet wasn`t found");
        }

        var processedDevices = new List<MiningDeviceInfo>();

        foreach (var deviceId in request.MiningDevices)
        {
            var device = await _miningDeviceRepository.GetActiveDeviceById(deviceId, request.UserId, cancellationToken);

            if (device is null)
            {
                continue;
            }

            if (flightSheet.IsDeviceSupport(device))
            {
                processedDevices.Add(device);
            }
        }

        var groupedDevices = processedDevices.Where(device => device.FlightSheetId != request.FightSheetId)
                                             .GroupBy(device => device.RigId);

        foreach (var rigDevices in groupedDevices)
        {
            // todo: отправка сообщения ригу
            await _mediator.Send(new SetFlightSheetCommand(request.FightSheetId, processedDevices.Select(device => device.Id).ToArray()),
                                 cancellationToken);
        }

        return Result<Guid[]>.Success(processedDevices.Select(device => device.Id).ToArray());
    }
}
