using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQueryHandler : IStreamRequestHandler<GetPoolsQuery, Pool>
{
    private readonly IMainRepository _repository;

    public GetPoolsQueryHandler(IMainRepository repository)
    {
        _repository = repository;
    }

    public IAsyncEnumerable<Pool> Handle(GetPoolsQuery request, CancellationToken cancellationToken)
    {
        return _repository.Pools.GetAll();
    }
}
