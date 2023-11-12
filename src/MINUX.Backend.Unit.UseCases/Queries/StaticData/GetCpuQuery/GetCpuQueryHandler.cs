using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Cpu;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCpuQuery;

public class GetCpuQueryHandler : IRequestHandler<GetCpuQuery, Result<Cpu>>
{
    public Task<Result<Cpu>> Handle(GetCpuQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
