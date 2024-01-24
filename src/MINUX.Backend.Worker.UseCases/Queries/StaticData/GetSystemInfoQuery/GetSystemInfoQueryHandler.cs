using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.GetSystemInfoQuery;

public class GetSystemInfoQueryHandler : IRequestHandler<GetSystemInfoQuery, Result<SystemInfo>>
{
    public Task<Result<SystemInfo>> Handle(GetSystemInfoQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
