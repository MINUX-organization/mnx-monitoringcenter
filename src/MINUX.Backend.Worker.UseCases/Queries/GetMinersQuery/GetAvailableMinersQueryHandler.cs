using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.GetMinersQuery;

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