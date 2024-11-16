using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetGpus;

/// <summary>
/// Запрос на получения списка видеокарт.
/// </summary>
public class GetGpusQuery : IStreamRequest<GetGpusQueryResponse>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public GetGpusQuery(Guid userId)
    {
        UserId = userId;
    }
}


/// <summary>
/// Обработчик <see cref="GetGpusQuery"/>.
/// </summary>
public class GetGpusQueryHandler : IStreamRequestHandler<GetGpusQuery, GetGpusQueryResponse>
{
    private readonly IMediator _mediator;

    public GetGpusQueryHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async IAsyncEnumerable<GetGpusQueryResponse> Handle(
        GetGpusQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var inventoryGpus = _mediator.CreateStream(new GetGpusDetailsQuery(request.UserId), cancellationToken)
                                     .ToBlockingEnumerable(cancellationToken)
                                     .ToList();

        var miningDevices = _mediator.CreateStream(
            new GetAvailableMiningDevicesQuery(request.UserId/*, inventoryGpus.Select(x => x.Id).ToArray()*/),
            cancellationToken);

        await foreach (var gpu in miningDevices)
        {
            var inventoryGpu = inventoryGpus.First(x => x.Id == gpu.Id);

            yield return new GetGpusQueryResponse()
            {
                Id = gpu.Id,
                Pci = inventoryGpu.Pci,
                Information = inventoryGpu.Information,
                RigName = inventoryGpu.RigName,
                DriverVersion = inventoryGpu.DriverVersion,
                FlightSheetName = gpu.FlightSheet?.Name,
                MinerName = gpu.FlightSheet?.Targets.First(x => x.DeviceType == MiningDeviceType.GPU).Miner?.Name
            };
        }
    }
}
