using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Contracts;

namespace MINUX.Backend.Unit.UseCases.Queries.GetStaticDataQuery;

public class GetStaticDataQueryHandler : IRequestHandler<GetStaticDataQuery, Result<StaticData>>
{
    public Task<Result<StaticData>> Handle(GetStaticDataQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
