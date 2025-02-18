using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Mining.Miner;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Miner.Queries;

using Miner = Core.Mining.Miner.Miner;

/// <summary>
/// Запрос на получение списка доступных майнеров.
/// </summary>
public sealed record GetAvailableMinersQuery : IStreamRequest<Miner>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetAvailableMinersQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    public GetAvailableMinersQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров.
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, Miner>
{
    private readonly IMinerRepository _repository;

    public GetAvailableMinersQueryHandler(IMinerRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Miner> Handle(GetAvailableMinersQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAvailableMiners(request.Specification);
    }
}