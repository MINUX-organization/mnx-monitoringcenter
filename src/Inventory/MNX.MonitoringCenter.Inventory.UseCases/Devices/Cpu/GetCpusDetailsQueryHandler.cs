using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusDetails;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;

/// <summary>
/// Обработчик <see cref="GetCpusDetailsQuery"/>.
/// </summary>
public class GetCpusDetailsQueryHandler : IStreamRequestHandler<GetCpusDetailsQuery, CpuDetails>
{
    private readonly ICpuRepository _cpuRepository;

    public GetCpusDetailsQueryHandler(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
    }

    public IAsyncEnumerable<CpuDetails> Handle(GetCpusDetailsQuery request, CancellationToken cancellationToken)
    {
        return _cpuRepository.GetCpus(request.Specification);
    }
}
