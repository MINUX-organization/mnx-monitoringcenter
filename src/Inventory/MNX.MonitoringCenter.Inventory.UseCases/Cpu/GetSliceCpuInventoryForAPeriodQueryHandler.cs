using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Queries.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

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
        return _cpuRepository.GetSliceForAPeriod(request.Specification, request.StartPeriod, request.EndPeriod);
    }
}
