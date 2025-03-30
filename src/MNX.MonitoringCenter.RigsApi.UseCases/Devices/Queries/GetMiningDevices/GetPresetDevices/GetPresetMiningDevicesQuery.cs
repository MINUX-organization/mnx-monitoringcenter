using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices.GetPresetDevices;

/// <summary>
/// Запрос на получение майнинг устройств, к которым применён пресет.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="PresetId"> Идентификатор пресета. </param>
public sealed record GetPresetMiningDevicesQuery(Guid UserId, Guid PresetId)
    : IStreamRequest<Group<Group<MiningDevice>>>;

/// <summary>
/// Обработчик <see cref="GetPresetMiningDevicesQuery"/>.
/// </summary>
public class GetPresetMiningDevicesQueryHandler
    : GetMiningDevicesBaseQueryHandler,
    IStreamRequestHandler<GetPresetMiningDevicesQuery, Group<Group<MiningDevice>>>
{
    public GetPresetMiningDevicesQueryHandler(IMediator mediator)
        : base(mediator) { }

    public IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(GetPresetMiningDevicesQuery request,
                                                               CancellationToken cancellationToken)
    {
        var filterString = "PresetId = @0";
        var filterParameters = new object[] { request.PresetId };

        var query = new GetAvailableMiningDevicesQuery(request.UserId,
                                                       filterString,
                                                       filterParameters);

        return Handle(query, cancellationToken);
    }
}