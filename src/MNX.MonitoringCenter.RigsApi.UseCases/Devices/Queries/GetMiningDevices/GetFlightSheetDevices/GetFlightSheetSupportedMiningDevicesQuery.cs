using MediatR;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices.GetFlightSheetDevices;

/// <summary>
/// Запрос на получение списка поддерживающихся полётным листом майнинг устройств,
/// сгруппированных сначала по ригу, а затем по типу.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="FlightSheetId"> Идентификатор полётного листа. </param>
public record GetFlightSheetSupportedMiningDevicesQuery(Guid UserId, Guid FlightSheetId)
    : IStreamRequest<Group<Group<MiningDevice>>>
{ }


/// <summary>
/// Обработчик <see cref="GetAvailableMiningDevicesQuery"/>.
/// </summary>
public class GetFlightSheetSupportedMiningDevicesQueryHandler : 
    GetMiningDevicesBaseQueryHandler,
    IStreamRequestHandler<GetFlightSheetSupportedMiningDevicesQuery, Group<Group<MiningDevice>>>
{
    public GetFlightSheetSupportedMiningDevicesQueryHandler(IMediator mediator)
        : base(mediator) { }

    public IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(
        GetFlightSheetSupportedMiningDevicesQuery request,
        CancellationToken cancellationToken)
    {
        var query = new GetFlightSheetSupportedDevicesQuery(request.UserId,
                                                            request.FlightSheetId);

        return Handle(query, cancellationToken);
    }
}
