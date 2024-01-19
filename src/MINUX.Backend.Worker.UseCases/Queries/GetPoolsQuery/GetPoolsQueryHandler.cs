using MediatR;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.UseCases.Abstractions;

namespace MINUX.Backend.Worker.UseCases.Queries.GetPoolsQuery;

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
