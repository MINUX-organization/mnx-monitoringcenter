using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters.Harddrive;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetHarddriveData;

/// <summary>
/// Обработчик получения информации о жёстких дисках
/// </summary>
public class GetHarddrivesDataQueryHandler : IStreamRequestHandler<GetHarddrivesDataQuery, Harddrive>
{
    private readonly IHardwareParametersRepository _repository;

    public GetHarddrivesDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<Harddrive> Handle(GetHarddrivesDataQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var item in await _repository.GetHarddrivesParameters())
        {
            yield return item;
        }
    }
}
