using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;
using MNX.MonitoringCenter.Management.UseCases.Mining.MiningDevice.Queries;
using MNX.Application.UseCases.Mediator;
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
        var inventoryCpus =
            await _mediator.GetListAsync(new GetCpusDetailsQuery(request.UserId), cancellationToken);

        var filterString = $"Id in ({string.Join(",", inventoryCpus.Select((cpu, index) => $"@{index}"))})";
        var filterParameters = inventoryCpus.Select(x => (object)x.Id).ToArray();

        var miningDevices = _mediator.CreateStream(
            new GetAvailableMiningDevicesQuery(request.UserId, filterString, filterParameters),
            cancellationToken);

        await foreach (var cpu in miningDevices.WithCancellation(cancellationToken))
        {
            // Берём последний CPU, так как он из последней инвентаризации.
            // Если риг выключили, то у него будет последняя актуальная инвентаризация,
            // пока его не включат, даже если CPU уже перенесли на другой риг.
            var inventoryCpu = inventoryCpus.LastOrDefault(x => x.Id == cpu.Id);

            yield return new GetCpusQueryResponse()
            {
                Id = cpu.Id,
                Pci = inventoryCpu?.Pci,
                Information = inventoryCpu?.Information,
                RigName = inventoryCpu?.RigName,
                FlightSheetName = cpu.FlightSheetName,
                PresetName = cpu.PresetName,
                MinerName = cpu.MinerName,
                IsOnline = cpu.IsOnline
            };
        }
    }
}
