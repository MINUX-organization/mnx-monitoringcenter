using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Gpu;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetGpuData;

/// <summary>
/// Обработчик запроса о получении параметров о видеокартах
/// </summary>
public class GetGpusDataQueryHandler : IStreamRequestHandler<GetGpusDataQuery, Gpu>
{
    private readonly IHardwareParametersRepository _repository;

    public GetGpusDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<Gpu> Handle(GetGpusDataQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var item in await _repository.GetGpusParameters())
        {
            yield return item;
        }
    }
}
