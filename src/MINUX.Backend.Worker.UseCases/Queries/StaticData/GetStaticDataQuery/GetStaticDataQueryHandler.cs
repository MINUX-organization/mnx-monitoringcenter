using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Contracts;

namespace MINUX.Backend.Worker.UseCases.Queries.GetStaticDataQuery;

public class GetStaticDataQueryHandler : IRequestHandler<GetStaticDataQuery, Result<StaticData>>
{
    public Task<Result<StaticData>> Handle(GetStaticDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
