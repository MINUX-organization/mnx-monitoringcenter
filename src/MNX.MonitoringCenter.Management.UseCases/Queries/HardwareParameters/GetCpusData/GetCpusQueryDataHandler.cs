using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Cpu;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetCpusData;

/// <summary>
/// Обработчик запроса на получение параметров о CPUs
/// </summary>
public class GetCpusQueryDataHandler : IStreamRequestHandler<GetCpusDataQuery, Cpu>
{
    private readonly IHardwareParametersRepository _repository;

    public GetCpusQueryDataHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<Cpu> Handle(GetCpusDataQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var item in await _repository.GetCpusParameters())
        {
            yield return item;
        }
    }
}
