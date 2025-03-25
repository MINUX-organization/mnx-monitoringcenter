using MediatR;
using MNX.Application.UseCases.Results;
using MNX.Application.UseCases.CommandValidation;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Commands.ConfirmFlightSheet;

/// <summary>
/// Подтвердить полётный лист на майнинг устройствах.
/// </summary>
/// <param name="MiningDevices"> Идентификаторы майнинг устройств. </param>
public sealed record ConfirmFlightSheetCommand(Guid[] MiningDevices)
    : IValidatableCommand<Unit>;


/// <summary>
/// Обработчик <see cref="ConfirmFlightSheetCommand"/>.
/// </summary>
public class ConfirmFlightSheetCommandHandler : IRequestHandler<ConfirmFlightSheetCommand, Result<Unit>>
{
    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public ConfirmFlightSheetCommandHandler(IMiningDeviceRepository miningDeviceRepository)
    {
        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async Task<Result<Unit>> Handle(ConfirmFlightSheetCommand request, CancellationToken cancellationToken)
    {
        await _miningDeviceRepository.ConfirmFlightSheet(request.MiningDevices);
        return Result<Unit>.Empty();
    }
}
