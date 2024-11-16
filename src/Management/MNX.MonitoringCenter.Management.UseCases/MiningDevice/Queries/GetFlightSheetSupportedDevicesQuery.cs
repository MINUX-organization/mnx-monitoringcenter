using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.FlightSheet;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;

/// <summary>
/// Получить поддерживаемые полётным листом майнинг устройства.
/// </summary>
/// <param name="UserId"></param>
/// <param name="FlightSheetId"></param>
public record GetFlightSheetSupportedDevicesQuery(Guid UserId, Guid FlightSheetId)
    : IStreamRequest<MiningDeviceModel>;


/// <summary>
/// Обработчик <see cref="GetAvailableMiningDevicesQuery"/>.
/// </summary>
public class GetFlightSheetSupportedDevicesQueryHandler
    : IStreamRequestHandler<GetFlightSheetSupportedDevicesQuery, MiningDeviceModel>
{
    private readonly IMapper _mapper;

    private readonly IFlightSheetRepository _flightSheetRepository;

    private readonly IMiningDeviceRepository _miningDeviceRepository;

    public GetFlightSheetSupportedDevicesQueryHandler(IMapper mapper,
                                                      IFlightSheetRepository flightSheetRepository,
                                                      IMiningDeviceRepository miningDeviceRepository)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        _flightSheetRepository = flightSheetRepository
            ?? throw new ArgumentNullException(nameof(flightSheetRepository));

        _miningDeviceRepository = miningDeviceRepository
            ?? throw new ArgumentNullException(nameof(miningDeviceRepository));
    }

    public async IAsyncEnumerable<MiningDeviceModel> Handle(GetFlightSheetSupportedDevicesQuery request,
                                                            [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var flightSheet = await _flightSheetRepository
            .GetAvailableById(request.FlightSheetId, request.UserId, cancellationToken);

        if (flightSheet is not null)
        {
            var devices = _miningDeviceRepository.GetFlightSheetSupportedDevices(flightSheet);

            await foreach (var device in devices.WithCancellation(cancellationToken))
            {
                yield return _mapper.Map<MiningDeviceModel>(device);
            }
        }
    }
}
