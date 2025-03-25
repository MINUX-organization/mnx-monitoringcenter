using MediatR;
using MNX.Application.UseCases.Results;
using MNX.MonitoringCenter.Management.Contracts.AlgorithmBinding;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Algorithm.Queries.GetAlgorithmById;

/// <summary>
/// Запрос на получения алгоритма с относительными наименованиями для майнеров.
/// </summary>
public record GetAlgorithmByIdQuery : IRequest<Result<AlgorithmBindingModel>>
{
    /// <summary>
    /// Идентификатор алгоритма.
    /// </summary>
    public Guid AlgorithmId { get; }

    /// <summary>
    /// Идентификатор пользователя.
    /// </summary>
    public Guid UserId { get; }

    public GetAlgorithmByIdQuery(Guid id, Guid userId)
    {
        AlgorithmId = id;
        UserId = userId;
    }
}