using MediatR;
using MNX.MonitoringCenter.Management.Core.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;

/// <summary>
/// Получить поддерживаемые полётным листом майнинг устройства.
/// </summary>
/// <param name="UserId"></param>
/// <param name="FlightSheetId"></param>
public record GetFlightSheetSupportedDevicesQuery(Guid UserId, Guid FlightSheetId)
    : IStreamRequest<MiningDeviceInfo>;


/// <summary>
/// Обработчик <see cref="GetAvailableMiningDevicesQuery"/>.
/// </summary>
public class GetFlightSheetSupportedDevicesQueryHandler
    : IStreamRequestHandler<GetFlightSheetSupportedDevicesQuery, MiningDeviceInfo>
{
    private readonly IFlightSheetRepository _flightSheetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetFlightSheetSupportedDevicesQueryHandler(IFlightSheetRepository flightSheetRepository,
                                                      IMiningDeviceRepository miningDeviceRepository)
    {
        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async IAsyncEnumerable<MiningDeviceInfo> Handle(GetFlightSheetSupportedDevicesQuery request,
                                                          [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository
            .GetAvailableById(request.FlightSheetId, request.UserId, cancellationToken);

        if (flightSheet is not null)
        {
            var devices = _miningDeviceRepository.GetFlightSheetSupportedDevices(flightSheet);

            await foreach (var device in devices)
            {
                yield return device;
            }
        }
    }
}
