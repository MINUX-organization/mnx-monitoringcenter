using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
using MNX.MonitoringCenter.Management.Core.FlightSheet.Target;
using MNX.MonitoringCenter.Management.Core.MiningDevice.Enums;
using MNX.MonitoringCenter.Management.UseCases.MiningDevice.Queries;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.RigsApi.UseCases.Devices.Queries.GetCpus;

/// <summary>
/// Запрос на получение списка процессоров.
/// </summary>
public class GetCpusQuery : IStreamRequest<GetCpusQueryResponse>
{
    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public GetCpusQuery(Guid userId)
    {
        UserId = userId;
    }
}

/// <summary>
/// Обработчик <see cref="GetCpusQuery"/>.
/// </summary>
public class GetCpusQueryHandler : IStreamRequestHandler<GetCpusQuery, GetCpusQueryResponse>
{
    private readonly IMediator _mediator;

    public GetCpusQueryHandler(IMediator mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    public async IAsyncEnumerable<GetCpusQueryResponse> Handle(
        GetCpusQuery request, [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var inventoryCpus = _mediator.CreateStream(new GetCpusDetailsQuery(request.UserId), cancellationToken)
                                     .ToBlockingEnumerable(cancellationToken)
                                     .ToList();

        var miningDevices = _mediator.CreateStream(
            new GetAvailableMiningDevicesQuery(request.UserId/*, inventoryCpus.Select(x => x.Id).ToArray()*/),
            cancellationToken);

        await foreach (var cpu in miningDevices)
        {
            var inventoryCpu = inventoryCpus.First(x => x.Id == cpu.Id);

            yield return new GetCpusQueryResponse()
            {
                Id = cpu.Id,
                Pci = inventoryCpu.Pci,
                Information = inventoryCpu.Information,
                RigName = inventoryCpu.RigName,
                FlightSheetName = cpu.FlightSheet?.Name,
                MinerName = cpu.FlightSheet?.Targets.First(x => x.DeviceType == MiningDeviceType.CPU).Miner?.Name
            };
        }
    }
}
