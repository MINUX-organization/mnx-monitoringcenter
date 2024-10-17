using MediatR;
using MNX.MonitoringCenter.Inventory.Contracts.Requests.Rigs.Devices.Cpu.GetCpusInfo;

namespace MNX.MonitoringCenter.Inventory.UseCases.Devices.Cpu;

/// <summary>
/// Обработчик <see cref="GetCpusInfoQuery"/>.
/// </summary>
public class GetCpusInfoQueryHandler : IStreamRequestHandler<GetCpusInfoQuery, CpuModel>
{
    private readonly ICpuRepository _cpuRepository;

    public GetCpusInfoQueryHandler(ICpuRepository cpuRepository)
    {
        _cpuRepository = cpuRepository ?? throw new ArgumentNullException(nameof(cpuRepository));
    }

    public IAsyncEnumerable<CpuModel> Handle(GetCpusInfoQuery request, CancellationToken cancellationToken)
    {
        return _cpuRepository.GetCpus(request.Specification);
    }
}
