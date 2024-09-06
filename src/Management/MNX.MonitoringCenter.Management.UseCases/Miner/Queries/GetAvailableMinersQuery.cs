using MediatR;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Miner.Queries;

/// <summary>
/// Запрос на получение списка доступных майнеров.
/// </summary>
public sealed record GetAvailableMinersQuery() : IStreamRequest<string>;

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров.
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, string>
{
    private readonly IMinerRepository _repository;

    public GetAvailableMinersQueryHandler(IMinerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async IAsyncEnumerable<string> Handle(GetAvailableMinersQuery request,
                                                [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var miner in _repository.GetAvailableMiners())
        {
            yield return miner.Name;
        }
    }
}