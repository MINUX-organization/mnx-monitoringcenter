using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
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
        var inventoryCpus = await GetInventoryCpus(request, cancellationToken);

        var filterString = string.Join(" or ", inventoryCpus.Select((cpu, index) => $"Id == @{index}"));
        var filterParameters = inventoryCpus.Select(x => (object)x.Id).ToArray();

        var miningDevices = _mediator.CreateStream(
            new GetAvailableMiningDevicesQuery(request.UserId, filterString, filterParameters),
            cancellationToken);

        await foreach (var cpu in miningDevices.WithCancellation(cancellationToken))
        {
            var inventoryCpu = inventoryCpus.First(x => x.Id == cpu.Id);

            yield return new GetCpusQueryResponse()
            {
                Id = cpu.Id,
                Pci = inventoryCpu.Pci,
                Information = inventoryCpu.Information,
                RigName = inventoryCpu.RigName,
                FlightSheetName = cpu.FlightSheetName,
                MinerName = cpu.MinerName
            };
        }
    }

    private async Task<List<CpuDetails>> GetInventoryCpus(GetCpusQuery request, CancellationToken cancellationToken)
    {
        var inventoryCpus = new List<CpuDetails>();

        var stream = _mediator.CreateStream(new GetCpusDetailsQuery(request.UserId), cancellationToken);

        await foreach (var cpu in stream)
        {
            inventoryCpus.Add(cpu);
        }

        return inventoryCpus;
    }
}
