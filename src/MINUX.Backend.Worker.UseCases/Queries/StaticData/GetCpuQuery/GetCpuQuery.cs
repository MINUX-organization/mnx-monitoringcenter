using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.StaticData.Cpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetCpuQuery;

public class GetCpuQuery : IRequest<Result<Cpu>>
{
}
