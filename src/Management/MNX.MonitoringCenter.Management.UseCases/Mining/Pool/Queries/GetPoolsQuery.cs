using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Mining.Pool.Queries;

/// <summary>
/// Запрос на получение списка пулов.
/// </summary>
public sealed record GetPoolsQuery : IStreamRequest<PoolModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    ///
    public GetPoolsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

    ///
    public GetPoolsQuery(Guid userId, string filterString, object[] filterParameters)
    {
        Specification = new Specification(userId, filterString, filterParameters);
    }
}

/// <summary>
/// Обработчик запроса на получение списка пулов. <see cref="GetPoolsQuery"/>.
/// </summary>
public class GetPoolsQueryHandler : IStreamRequestHandler<GetPoolsQuery, PoolModel>
{
    private readonly IPoolRepository _repository;

    private readonly IPoolMapper _poolMapper;

    ///
    public GetPoolsQueryHandler(IPoolRepository repository, IPoolMapper poolMapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _poolMapper = poolMapper ?? throw new ArgumentNullException(nameof(poolMapper));
    }

    ///
    public async IAsyncEnumerable<PoolModel> Handle(GetPoolsQuery request,
                                                   [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        var pools = _repository.GetAllAvailable(request.Specification);

        await foreach (var pool in pools.WithCancellation(cancellationToken))
        {
            yield return _poolMapper.MapToModel(pool);
        }
    }
}
