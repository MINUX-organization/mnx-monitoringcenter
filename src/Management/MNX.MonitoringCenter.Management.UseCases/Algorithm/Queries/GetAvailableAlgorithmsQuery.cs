using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Algorithm.Queries;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgorithmsQuery() : IStreamRequest<Core.Algorithm>;

/// <summary>
/// Обработчик запроса на получение доступных алгоритмов
/// </summary>
public class GetAvailableAlgorithmsQueryHandler : IStreamRequestHandler<GetAvailableAlgorithmsQuery, Core.Algorithm>
{
    private readonly IAlgorithmRepository _repository;

    public GetAvailableAlgorithmsQueryHandler(IAlgorithmRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Core.Algorithm> Handle(GetAvailableAlgorithmsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetNamesOfAvailableAlgorithms();
    }
}