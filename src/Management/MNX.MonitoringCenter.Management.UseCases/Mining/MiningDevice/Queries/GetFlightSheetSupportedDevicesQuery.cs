using MediatR;
using AutoMapper;
using System.Runtime.CompilerServices;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.Mining.FlightSheet;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

/// <summary>
/// Получить поддерживаемые полётным листом майнинг устройства.
/// </summary>
public record GetFlightSheetSupportedDevicesQuery
    : IUserableStreamRequest<MiningDeviceModel>
{
    /// <summary>
    /// Пользовательский идентификатор.
    /// </summary>
    public Guid UserId { get => Specification.UserId; }

    /// <summary>
    /// Идентификатор полётного листа.
    /// </summary>
    public Guid FlightSheetId { get; init; }

    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; init; }

    public GetFlightSheetSupportedDevicesQuery(Guid userId, Guid flightSheetId)
    {
        FlightSheetId = flightSheetId;
        Specification = new Specification(userId);
    }

    public GetFlightSheetSupportedDevicesQuery(Guid userId,
                                               Guid flightSheetId,
                                               string filterString,
                                               object[] filterParameters)
    {
        FlightSheetId = flightSheetId;
        Specification = new Specification(userId, filterString, filterParameters);
    }
}


/// <summary>
/// Обработчик <see cref="GetFlightSheetSupportedDevicesQuery"/>.
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
            .GetAvailableById(request.FlightSheetId, request.Specification.UserId, cancellationToken);

        if (flightSheet is not null)
        {
            var devices = _miningDeviceRepository.GetAvailable(request.Specification);

            await foreach (var device in devices.WithCancellation(cancellationToken))
            {
                if (flightSheet.IsDeviceSupport(device))
                {
                    yield return _mapper.Map<MiningDeviceModel>(device);
                }
            }
        }
    }
}
