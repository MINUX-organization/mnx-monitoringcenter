using MediatR;
using MNX.Application.UseCases.Mediator;
using MNX.Application.UseCases.Requests;
using MNX.MonitoringCenter.Inventory.Contracts;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.Contracts.MiningDevice;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetMiningDevices;

/// <summary>
/// Базовый обработчик получения списка майнинг устройств.
/// </summary>
public abstract class GetMiningDevicesBaseQueryHandler
{
    private readonly IMediator _mediator;

    private List<GpuDetails>? _inventoryGpus;

    private HashSet<Rig>? _rigs;

    public GetMiningDevicesBaseQueryHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async IAsyncEnumerable<Group<Group<MiningDevice>>> Handle(
        IUserableStreamRequest<MiningDeviceModel> request,
        [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var stream = _mediator.CreateStream(request,
                                            cancellationToken);

        var devices = new List<MiningDevice>();

        await foreach (var device in stream.WithCancellation(cancellationToken))
        {
            devices.Add(await MapDevice(request.UserId, device, cancellationToken));
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

    private async Task<MiningDevice> MapDevice(Guid userId, MiningDeviceModel model,
                                               CancellationToken cancellationToken)
    {
        var rigs = await GetRigs(userId, cancellationToken);

        var device = new MiningDevice()
        {
            Id = model.Id,
            Manufacturer = model.Manufacturer,
            Model = model.Model,
            Type = model.Type,
            RigName = rigs.First(rig => rig.Id == model.RigId).Name,
            FlightSheetName = model.FlightSheetName,
            FlightSheetIsConfirm = model.FlightSheetIsConfirm,
            MinerName = model.MinerName,
        };

        if (device.Type == "GPU")
        {
            var inventoryGpus = await GetInventoryGpus(userId, cancellationToken);
            device.PciBus = inventoryGpus.Last(x => x.Id == device.Id).Pci.Bus;
        }

        return device;
    }

    private async Task<List<GpuDetails>> GetInventoryGpus(Guid userId, CancellationToken cancellationToken)
    {
        return _inventoryGpus ??=
            await _mediator.GetListAsync(new GetGpusDetailsQuery(userId), cancellationToken);
    }

    private async Task<HashSet<Rig>> GetRigs(Guid userId, CancellationToken cancellationToken)
    {
        return _rigs ??= await _mediator.GetHashSetAsync(new GetRigsQuery(userId), cancellationToken);
    }
}
