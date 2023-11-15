using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetMinersQuery;

/// <summary>
/// Обработчик запроса на получения списка доступных майнеров
/// </summary>
public class GetAvailableMinersQueryHandler : IStreamRequestHandler<GetAvailableMinersQuery, Miner>
{
    private readonly IMainRepository _repository;

    public GetAvailableMinersQueryHandler(IMainRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Miner> Handle(GetAvailableMinersQuery request, CancellationToken cancellationToken)
    {
        return _repository.Miners.GetAvailableMiners();
    }
}