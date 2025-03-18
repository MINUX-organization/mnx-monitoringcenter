using MediatR;
using MNX.Application.UseCases.Mediator;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Gpu.GetGpusDetails;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;
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
        var inventoryGpus =
            await _mediator.GetListAsync(new GetGpusDetailsQuery(request.UserId), cancellationToken);

        var filterString = $"Id in ({string.Join(",", inventoryGpus.Select((cpu, index) => $"@{index}"))})";
        var filterParameters = inventoryGpus.Select(x => (object)x.Id).ToArray();

        var miningDevices = _mediator.CreateStream(
            new GetAvailableMiningDevicesQuery(request.UserId, filterString, filterParameters),
            cancellationToken);

        await foreach (var gpu in miningDevices.WithCancellation(cancellationToken))
        {
            // Берём последнюю карту, так как она из последней инвентаризации.
            // Если риг выключили, то у него будет последняя актуальная инвентаризация,
            // пока его не включат, даже если карты уже перенесли на другой риг.
            var inventoryGpu = inventoryGpus.Last(x => x.Id == gpu.Id);

            yield return new GetGpusQueryResponse()
            {
                Id = gpu.Id,
                Pci = inventoryGpu.Pci,
                Information = inventoryGpu.Information,
                RigName = inventoryGpu.RigName,
                DriverVersion = inventoryGpu.DriverVersion,
                FlightSheetName = gpu.FlightSheetName,
                MinerName = gpu.MinerName
            };
        }
    }
}
