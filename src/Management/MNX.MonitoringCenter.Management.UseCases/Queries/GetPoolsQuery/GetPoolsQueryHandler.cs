using AutoMapper;
using MediatR;
using MNX.MonitoringCenter.Management.Contracts;
using MNX.MonitoringCenter.Management.UseCases.Abstractions;
using System.Runtime.CompilerServices;

namespace MNX.MonitoringCenter.Management.UseCases.Queries.GetPoolsQuery;

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
        await foreach (var pool in _repository.GetAllAvailable(request.UserId))
        {
            yield return _mapper.Map<PoolModel>(pool);
        }
    }
}
