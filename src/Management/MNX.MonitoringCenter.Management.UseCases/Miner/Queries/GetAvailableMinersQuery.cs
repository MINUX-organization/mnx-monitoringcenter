using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Miner.Queries;

/// <summary>
/// Запрос на получение списка доступных майнеров.
/// </summary>
public sealed record GetAvailableMinersQuery : IStreamRequest<Core.Miner>;

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров.
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, Core.Miner>
{
    private readonly IMinerRepository _repository;

    public GetAvailableMinersQueryHandler(IMinerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Core.Miner> Handle(GetAvailableMinersQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAvailableMiners();
    }
}