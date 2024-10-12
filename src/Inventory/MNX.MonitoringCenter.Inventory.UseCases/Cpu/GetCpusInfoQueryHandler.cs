using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Queries.Cpu;

namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

/// <summary>
/// Обработчик <see cref="GetCpusInfoQuery"/>.
/// </summary>
public class GetCpusInfoQueryHandler : IStreamRequestHandler<GetCpusInfoQuery, Cpu>
{
    private readonly ICpuRepository _cpuRepository;

    public GetCpusInfoQueryHandler(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
    }

    public IAsyncEnumerable<Cpu> Handle(GetCpusInfoQuery request, CancellationToken cancellationToken)
    {
        return _cpuRepository.GetList(request.Specification);
    }
}
