using MediatR;
using MNX.MonitoringCenter.Management.Core.HardwareParameters;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.HardwareParameters.GetRamsData;

/// <summary>
/// Обработчик запроса на получение информации о плашках оперативной памяти
/// </summary>
public class GetRamsDataQueryHandler : IStreamRequestHandler<GetRamsDataQuery, Ram>
{
    private readonly IHardwareParametersRepository _repository;

    public GetRamsDataQueryHandler(IHardwareParametersRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<Ram> Handle(GetRamsDataQuery request, [EnumeratorCancellation]CancellationToken cancellationToken)
    {
        foreach (var item in await _repository.GetRamsParameters())
        {
            yield return item;
        }
    }
}
