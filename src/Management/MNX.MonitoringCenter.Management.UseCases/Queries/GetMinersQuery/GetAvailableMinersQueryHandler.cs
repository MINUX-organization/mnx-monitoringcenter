using MediatR;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetMinersQuery;

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров
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
        return _repository.GetAvailableMiners();
    }
}