using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices.GetFlightSheetDevices;

/// <summary>
/// Запрос на получение майнинг устройств, к которым применён полётный лист.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="FlightSheetId"> Идентификатор полётного листа. </param>
public sealed record GetFlightSheetMiningDevicesQuery(Guid UserId, Guid FlightSheetId)
    : IStreamRequest<Group<Group<MiningDevice>>>;

/// <summary>
/// Обработчик <see cref="GetFlightSheetMiningDevicesQuery"/>.
/// </summary>
public class GetFlightSheetMiningDevicesQueryHandler :
    GetMiningDevicesBaseQueryHandler,
    IStreamRequestHandler<GetFlightSheetMiningDevicesQuery, Group<Group<MiningDevice>>>
{
    public GetFlightSheetMiningDevicesQueryHandler(IMediator mediator)
        : base(mediator) { }

    public IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(GetFlightSheetMiningDevicesQuery request,
                                                               CancellationToken cancellationToken)
    {
        var filterString = "FlightSheetId == @0";
        var filterParameters = new object[] { request.FlightSheetId };

        var query = new GetAvailableMiningDevicesQuery(request.UserId,
                                                       filterString,
                                                       filterParameters);

        return Handle(query, cancellationToken);
    }
}
