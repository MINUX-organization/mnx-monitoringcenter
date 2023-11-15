using MediatR;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetAlgorithmsQuery;

/// <summary>
/// Обработчик запроса на получение доступных алгоритмов
/// </summary>
public class GetAvailableAlgorithmsQueryHandler : IStreamRequestHandler<GetAvailableAlgoritmsQuery, string>
{
    private readonly IMainRepository _repository;

    public GetAvailableAlgorithmsQueryHandler(IMainRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<string> Handle(GetAvailableAlgoritmsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Algorithms.GetNamesOfAvailableAlgorithms();
    }
}