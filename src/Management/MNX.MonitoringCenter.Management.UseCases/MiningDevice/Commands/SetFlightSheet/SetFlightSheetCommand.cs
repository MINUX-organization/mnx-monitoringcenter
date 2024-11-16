using MediatR;
using MNX.Application.UseCases;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Commands.SetFlightSheet;

/// <summary>
/// Установить на майнинг устройства полётный лист.
/// </summary>
/// <param name="FightSheetId"> Идентификатор полётного листа. </param>
/// <param name="MiningDevices"> Майнинг устройства. </param>
public sealed record SetFlightSheetCommand(Guid FightSheetId, Guid[] MiningDevices)
    : IRequest<Result<Unit>>;


/// <summary>
/// Реализация <see cref="SetFlightSheetCommand"/>.
/// </summary>
public class SetFlightSheetCommandHandler : IRequestHandler<SetFlightSheetCommand, Result<Unit>>
{
    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public SetFlightSheetCommandHandler(IMiningDeviceRepository miningDeviceRepository)
    {
        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Unit>> Handle(SetFlightSheetCommand request, CancellationToken cancellationToken)
    {
        await _miningDeviceRepository.SetFlightSheet(request.MiningDevices, request.FightSheetId, cancellationToken);
        return Result<Unit>.Empty();
    }
}
