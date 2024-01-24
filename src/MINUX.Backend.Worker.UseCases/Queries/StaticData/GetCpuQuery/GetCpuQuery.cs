using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters.Cpu;

namespace MINUX.Backend.Worker.UseCases.Queries.GetCpuQuery;

public class GetCpuQuery : IRequest<Result<Cpu>>
{
}
