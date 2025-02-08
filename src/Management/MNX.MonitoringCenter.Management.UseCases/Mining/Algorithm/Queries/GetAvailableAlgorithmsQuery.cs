using MediatR;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries;

/// <summary>
/// Запрос на получение доступных алгоритмов
/// </summary>
public sealed record GetAvailableAlgorithmsQuery : IStreamRequest<Core.Mining.Algorithm>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetAvailableAlgorithmsQuery(Guid userId, string searchString)
    {
        Specification = new Specification(userId, searchString);
    }

    public GetAvailableAlgorithmsQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик запроса на получение доступных алгоритмов
/// </summary>
public class GetAvailableAlgorithmsQueryHandler : IStreamRequestHandler<GetAvailableAlgorithmsQuery, Core.Mining.Algorithm>
{
    private readonly IAlgorithmRepository _repository;

    public GetAvailableAlgorithmsQueryHandler(IAlgorithmRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Core.Mining.Algorithm> Handle(GetAvailableAlgorithmsQuery request,
                                                                CancellationToken cancellationToken)
    {
        return _repository.GetNamesOfAvailableAlgorithms(request.Specification);
    }
}