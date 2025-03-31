using MediatR;
using MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

namespace MNX.MonitoringCenter.Management.UseCases.Overclocking.Presets.Queries;

/// <summary>
/// Получить поддерживаемые пресетом майнинг устройства.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="PresetId"> Идентификатор пресета. </param>
public record GetPresetSupportedMiningDevicesQuery(Guid UserId, Guid PresetId)
    : IStreamRequest<Group<Group<MiningDevice>>>;

/// <summary>
/// Обработчик <see cref="GetPresetSupportedMiningDevicesQuery"/>.
/// </summary>
public class GetPresetSupportedMiningDevicesQueryHandler :
    GetMiningDevicesBaseQueryHandler,
    IStreamRequestHandler<GetPresetSupportedMiningDevicesQuery, Group<Group<MiningDevice>>>
{
    public GetPresetSupportedMiningDevicesQueryHandler(IMediator mediator)
        : base(mediator) { }

    public IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(GetPresetSupportedMiningDevicesQuery request,
                                                               CancellationToken cancellationToken)
    {
        var query = new GetPresetSupportedDevicesQuery(request.UserId, request.PresetId);

        return Handle(query, cancellationToken);
    }
}