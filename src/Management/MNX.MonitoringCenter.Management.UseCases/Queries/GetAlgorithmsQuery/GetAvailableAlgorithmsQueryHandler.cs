using MediatR;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetAlgorithmsQuery;

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