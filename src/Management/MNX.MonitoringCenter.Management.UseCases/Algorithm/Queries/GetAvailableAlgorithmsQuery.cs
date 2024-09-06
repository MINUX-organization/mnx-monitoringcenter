using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Algorithm.Queries;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgorithmsQuery() : IStreamRequest<string>;

/// <summary>
/// Обработчик запроса на получение доступных алгоритмов
/// </summary>
public class GetAvailableAlgorithmsQueryHandler : IStreamRequestHandler<GetAvailableAlgorithmsQuery, string>
{
    private readonly IAlgorithmRepository _repository;

    public GetAvailableAlgorithmsQueryHandler(IAlgorithmRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<string> Handle(GetAvailableAlgorithmsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetNamesOfAvailableAlgorithms();
    }
}