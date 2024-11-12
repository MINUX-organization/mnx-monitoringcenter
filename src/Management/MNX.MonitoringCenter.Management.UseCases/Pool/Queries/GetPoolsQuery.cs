using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Pool.Queries;

/// <summary>
/// Запрос на получение списка пулов.
/// </summary>
public sealed record GetPoolsQuery : IStreamRequest<PoolModel>
{
    /// <summary>
    /// Спецификация.
    /// </summary>
    public Specification Specification { get; }

    public GetPoolsQuery(Guid userId)
    {
        Specification = new Specification(userId);
    }

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

    private readonly IMapper _mapper;

    public GetPoolsQueryHandler(IPoolRepository repository, IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async IAsyncEnumerable<PoolModel> Handle(GetPoolsQuery request,
                                                   [EnumeratorCancellation] CancellationToken cancellationToken)
    {
        await foreach (var pool in _repository.GetAllAvailable(request.Specification).WithCancellation(cancellationToken))
        {
            yield return _mapper.Map<PoolModel>(pool);
        }
    }
}
