using MediatR;

namespace MNX.MonitoringCenter.Inventory.UseCases.Cpu;

using Cpu = Contracts.Cpu.Cpu;

/// <summary>
/// Запрос на получение списка процессоров.
/// </summary>
/// <param name="Specification"></param>
public sealed record GetCpusInfoQuery(DeviceSpecification Specification) : IStreamRequest<Cpu>;

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
