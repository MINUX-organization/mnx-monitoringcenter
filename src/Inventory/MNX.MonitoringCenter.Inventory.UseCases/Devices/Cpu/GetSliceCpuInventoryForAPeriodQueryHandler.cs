using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;

using Cpu = Contracts.Devices.Cpu.Cpu;

/// <summary>
/// Обработчик <see cref="GetSliceCpuInventoryForAPeriodQuery"/>.
/// </summary>
public class GetSliceCpuInventoryForAPeriodQueryHandler
    : IStreamRequestHandler<GetSliceCpuInventoryForAPeriodQuery, List<Cpu>>
{
    private readonly ICpuRepository _cpuRepository;

    public GetSliceCpuInventoryForAPeriodQueryHandler(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
    }

    public IAsyncEnumerable<List<Cpu>> Handle(GetSliceCpuInventoryForAPeriodQuery request,
                                              CancellationToken cancellationToken)
    {
        return _cpuRepository.GetCpusSliceForAPeriod(request.Specification, request.StartPeriod, request.EndPeriod);
    }
}
