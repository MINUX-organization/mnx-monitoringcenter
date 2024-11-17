using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

/// <summary>
/// Запрос на получение списка поддерживающихся полётным листом майнинг устройств,
/// сгруппированных сначала по ригу, а затем по типу.
/// </summary>
/// <param name="UserId"> Идентификатор пользователя. </param>
/// <param name="FlightSheetId"> Идентификатор полётного листа. </param>
public record GetFlightSheetSupportedMiningDevicesQuery(Guid UserId, Guid FlightSheetId)
    : IStreamRequest<Group<Group<MiningDevice>>> { }


/// <summary>
/// Обработчик <see cref="GetAvailableMiningDevicesQuery"/>.
/// </summary>
public class GetFlightSheetSupportedMiningDevicesQueryHandler
    : IStreamRequestHandler<GetFlightSheetSupportedMiningDevicesQuery, Group<Group<MiningDevice>>>
{
    private readonly IMediator _mediator;

    private List<GpuDetails>? _inventoryGpus;

    private List<Rig>? _rigs;

    public GetFlightSheetSupportedMiningDevicesQueryHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(
        GetFlightSheetSupportedMiningDevicesQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var devices = new List<MiningDevice>();

        var stream = _mediator.CreateStream(new GetFlightSheetSupportedDevicesQuery(request.UserId, request.FlightSheetId),
                                            cancellationToken);

        await foreach (var device in stream.WithCancellation(cancellationToken))
        {
            devices.Add(await MapDevice(request.UserId, device));
        }

        foreach (var group in devices.GroupBy(x => x.RigName))
        {
            yield return GroupDevicesByType(group);
        }
    }

    private static Group<Group<MiningDevice>> GroupDevicesByType(IGrouping<string, MiningDevice> group)
    {
        return new Group<Group<MiningDevice>>
        {
            Name = group.Key.ToString(),
            Elements = group.GroupBy(device => device.Type)
                            .Select(x => new Group<MiningDevice>()
                            {
                                Name = x.Key,
                                Elements = x.ToList()
                            })
                            .ToList()
        };
    }

    private async Task<MiningDevice> MapDevice(Guid userId, MiningDeviceModel model)
    {
        var rigs = await GetRigs(userId);

        var device = new MiningDevice()
        {
            Id = model.Id,
            Type = model.Type,
            RigName = rigs.First(rig => rig.Id == model.RigId).Name,
            FlightSheetName = model.FlightSheetName,
            MinerName = model.MinerName,
        };

        if (device.Type == "GPU")
        {
            var inventoryGpus = await GetInventoryGpus(userId);
            device.PciBus = inventoryGpus.First(x => x.Id == device.Id).Pci.Bus;
        }

        return device;
    }

    private async Task<List<GpuDetails>> GetInventoryGpus(Guid userId, CancellationToken cancellationToken = default)
    {
        if (_inventoryGpus is null)
        {
            _inventoryGpus = new List<GpuDetails>();

            var stream = _mediator.CreateStream(new GetGpusDetailsQuery(userId), cancellationToken);

            await foreach (var cpu in stream)
            {
                _inventoryGpus.Add(cpu);
            }
        }

        return _inventoryGpus;
    }

    private async Task<List<Rig>> GetRigs(Guid userId, CancellationToken cancellationToken = default)
    {
        if (_rigs is null)
        {
            _rigs = new List<Rig>();

            var stream = _mediator.CreateStream(new GetRigsQuery(userId), cancellationToken);

            await foreach (var rig in stream)
            {
                _rigs.Add(rig);
            }
        }

        return _rigs;
    }
}
