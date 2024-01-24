using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetCpuQuery;

public class GetCpuQueryHandler : IRequestHandler<GetCpuQuery, Result<Cpu>>
{
    public Task<Result<Cpu>> Handle(GetCpuQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
