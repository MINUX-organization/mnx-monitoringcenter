using MediatR;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.UseCases.Abstractions;

namespace MINUX.Backend.Unit.UseCases.Queries.GetPoolsQuery;

public class GetPoolsQueryHandler : IStreamRequestHandler<GetPoolsQuery, Pool>
{
    private readonly IPoolRepository _repository;

    public GetPoolsQueryHandler(IPoolRepository repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public IAsyncEnumerable<Pool> Handle(GetPoolsQuery request, CancellationToken cancellationToken)
    {
        return _repository.GetAll();
    }
}
