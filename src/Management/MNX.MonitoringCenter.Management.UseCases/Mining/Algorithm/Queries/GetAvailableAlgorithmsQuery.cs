using MediatR;
using System.Runtime.CompilerServices;

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

    public GetAvailableAlgorithmsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    public GetAvailableAlgorithmsQuery(Guid userId,
                                       string filterString,
                                       object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик запроса на получение доступных алгоритмов
/// </summary>
public class GetAvailableAlgorithmsQueryHandler : IStreamRequestHandler<GetAvailableAlgorithmsQuery, Core.Mining.Algorithm>
{
    private readonly IAlgorithmRepository _algorithmRepository;

    public GetAvailableAlgorithmsQueryHandler(IAlgorithmRepository repository)
    {
        _algorithmRepository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Core.Mining.Algorithm> Handle(GetAvailableAlgorithmsQuery request,
                                                          CancellationToken cancellationToken)
    {
        return _algorithmRepository.GetAvailable(request.Specification);
    }
}