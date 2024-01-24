using Kernel.UseCases;
using MediatR;
using MINUX.Backend.Worker.Core.HardwareParameters;

namespace MINUX.Backend.Worker.UseCases.Queries.GetSystemInfoQuery;

public class GetSystemInfoQuery : IRequest<Result<SystemInfo>>
{
}
