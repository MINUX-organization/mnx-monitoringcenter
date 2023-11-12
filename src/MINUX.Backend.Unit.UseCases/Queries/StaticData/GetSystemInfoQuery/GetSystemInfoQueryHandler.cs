using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData;

namespace MINUX.Backend.Unit.UseCases.Queries.GetSystemInfoQuery;

public class GetSystemInfoQueryHandler : IRequestHandler<GetSystemInfoQuery, Result<SystemInfo>>
{
    public Task<Result<SystemInfo>> Handle(GetSystemInfoQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
