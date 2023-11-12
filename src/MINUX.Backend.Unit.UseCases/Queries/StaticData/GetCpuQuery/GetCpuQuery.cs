using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Unit.Core.StaticData.Cpu;

namespace MINUX.Backend.Unit.UseCases.Queries.GetCpuQuery;

public class GetCpuQuery : IRequest<Result<Cpu>>
{
}
